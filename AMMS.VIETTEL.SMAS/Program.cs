using AMMS.VIETTEL.SMAS.Applications.Extensions;
using AMMS.VIETTEL.SMAS.Cores.Identity.Entities;
using AMMS.VIETTEL.SMAS.Helps.Authorizations;
using AMMS.VIETTEL.SMAS.Infratructures.Databases;
using Hangfire;
using Hangfire.MySql;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Share.WebApp.Controllers;
using Share.WebApp.Settings;
using Shared.Core.Loggers;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

IServiceCollection services = builder.Services;
IConfiguration configuration = builder.Configuration;
IConfigurationRoot configRoot = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

services.AddOptions(); //Kích hoạt Options
services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        //TODO read the same from settings for prod deployment
        policy
        .AllowAnyHeader()
        .AllowAnyMethod()
        //.AllowAnyOrigin()
        .SetIsOriginAllowed((host) => true)
        .AllowCredentials()
        ;
    });
});
services.AddVersion();// Versioning

services.AddApplicationServices();
services.AddDbContext(configuration);

AppSettings appSettings = new AppSettings();
configuration.Bind(appSettings);
AuthBaseController.AMMS_Master_HostAddress = builder.Configuration["Authentication:Authority"];

//Hangfire MySQL Server
services.AddHangfire(configuration => configuration
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseStorage(
        new MySqlStorage(
            builder.Configuration["ConnectionStrings:HangfireDBConnection"],
            new MySqlStorageOptions
            {
                QueuePollInterval = TimeSpan.FromSeconds(10),
                JobExpirationCheckInterval = TimeSpan.FromHours(1),
                CountersAggregateInterval = TimeSpan.FromMinutes(5),
                PrepareSchemaIfNecessary = true,
                DashboardJobListLimit = 5000,
                TransactionTimeout = TimeSpan.FromMinutes(1),
                TablesPrefix = "Hangfire",
            }
        )
    ));


services.AddHangfireServer();

services.AddIdentityCore<ApplicationUser>(options => {
    // Cấu hình các tùy chọn Identity, nếu cần
})
.AddRoles<IdentityRole>() // Nếu có sử dụng role
.AddEntityFrameworkStores<ViettelDbContext>();

// Add Auth
services.Configure<Authentication>(configuration.GetSection("Authentication"));
services.AddAuthentication("Bearer")
.AddJwtBearer("Bearer", options =>
{
    options.Authority = builder.Configuration["Authentication:Authority"];
    options.RequireHttpsMetadata = true;

    options.Audience = "Device";
});


services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
})
.AddCookie("Cookies", options =>
{
    options.AccessDeniedPath = "/account/denied";
})
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = builder.Configuration["Authentication:Authority"];
        options.RequireHttpsMetadata = true;
        options.ClientId = builder.Configuration["Authentication:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:ClientSecret"];
        options.ResponseType = builder.Configuration["Authentication:ResponseType"];



        options.Scope.Add("masterapi");
        options.Scope.Add("amms.viettel");


        //options.Scope.Add("profile");
        //options.Scope.Add("offline_access");

        options.GetClaimsFromUserInfoEndpoint = true;
        options.SaveTokens = true;

        //options.RefreshInterval = new TimeSpan(0, 0, 10);
        //options.SignedOutRedirectUri = "signout-callback-oidc"; //Sau khi đăng xuất sẽ trở về trang này

        options.UsePkce = true; //Kiểm tra toke trên https://jwt.io/ sẽ Signature valid
        options.ClaimActions.MapJsonKey("role", "role", "role");
        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = "name",
            RoleClaimType = "role"
        };
    })
    ;
var multiSchemePolicy = new AuthorizationPolicyBuilder(
    CookieAuthenticationDefaults.AuthenticationScheme,
    JwtBearerDefaults.AuthenticationScheme)
  .RequireAuthenticatedUser()
  .Build();
services.AddAuthorization(options =>
{
    options.DefaultPolicy = multiSchemePolicy;
    options.AddPolicy("Bearer", policy => policy.RequireClaim("scope", "amms.viettel"));
});

// Add services to the container.

builder.Services.AddControllers();
services.AddEndpointsApiExplorer();

//AutoMapper
services.AddAutoMapper(typeof(Program));
//services.AddAddAutoMapperServices();

//services.AddEventBusService(configuration);
//services.AddCaheService(configuration);

//ScopedServices
services.AddScopedServices();

//Swagger
//services.AddSwaggerGen();
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(
               "v1",
           new OpenApiInfo()
           {
               Title = appSettings.ApplicationDetail.ApplicationName.ToString(),
               Version = "1",
               Description = appSettings.ApplicationDetail.Description,
               Contact = new OpenApiContact()
               {
                   Email = "info@amms.acs.vn",
                   Name = "AMMS Solution",
                   Url = new Uri(appSettings.ApplicationDetail.ContactWebsite),
               },
               License = new OpenApiLicense()
               {
                   Name = "MIT License",
                   Url = new Uri(appSettings.ApplicationDetail.LicenseDetail)
               }
           });
    c.SwaggerDoc(
       "v2",
   new OpenApiInfo()
   {
       Title = appSettings.ApplicationDetail.ApplicationName.ToString(),
       Version = "2",
       Description = appSettings.ApplicationDetail.Description,
       Contact = new OpenApiContact()
       {
           Email = "info@amms.acs.vn",
           Name = "AMMS Solution",
           Url = new Uri(appSettings.ApplicationDetail.ContactWebsite)
       },
       License = new OpenApiLicense()
       {
           Name = "MIT License",
           Url = new Uri(appSettings.ApplicationDetail.LicenseDetail)
       }
   });


    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            Implicit = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri(builder.Configuration["Authentication:Authority"] + "/connect/authorize"),
                Scopes = new Dictionary<string, string> { { "amms.viettel", "AMMS Viettel SMAS" } }
            },
        },
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            new List<string>{ "amms.viettel" }
        }
    });

    // Add comment
    string fileName = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
    if (!File.Exists(fileName))
    {
        var myFile = File.Create(fileName);
        var content = $"<?xml version=\"1.0\"?>\r\n<doc>\r\n    <assembly>\r\n        <name>{Assembly.GetExecutingAssembly().GetName().Name}</name>\r\n    </assembly>\r\n    <members>\r\n \r\n    </members>\r\n</doc>\r\n";
        var contentarr = Encoding.ASCII.GetBytes(content);
        myFile.Write(contentarr, 0, contentarr.Count());
        myFile.Close();
    }
    c.IncludeXmlComments(fileName);

});

services.AddHttpClient();
services.AddControllersWithViews();

var app = builder.Build();

app.UseCors("CorsPolicy");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    //app.UseSwaggerUI();
    app.UseSwaggerUI(c =>
    {
        c.OAuthClientId(configuration["Authentication:Swagger:ClientId"]);
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AMMS Viettel SMAS Api v1");
    });

    app.UseDeveloperExceptionPage();
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();
app.MapDefaultControllerRoute();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    )
    .RequireAuthorization()
    ;

});

//Tạo các job chạy tự động, theo dõi trạng thái của các job     
//app.UseHangfireDashboard("/hangfire_dashboard");
app.UseHangfireDashboard("/hangfire_dashboard", new DashboardOptions
{
    IgnoreAntiforgeryToken = true,
    Authorization = new[] { new DashboardNoAuthorizationFilter() }
});
//app.UseHangfireDashboard();
app.UseHangfireServer();

using (var scope = app.Services.CreateScope())
{
    try
    {
        //var conJobService = scope.ServiceProvider.GetRequiredService<ICronJobService>();
        //RecurringJob.AddOrUpdate("Test", () => conJobService.Write(), "*/1 * * * *", TimeZoneInfo.Local);
        //RecurringJob.AddOrUpdate("CheckDeviceOnline" ,() => conJobService.CheckDeviceOnline(), "*/5 * * * *", TimeZoneInfo.Local);
    }
    catch (Exception e)
    {
        Logger.Error(e);
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.Run();