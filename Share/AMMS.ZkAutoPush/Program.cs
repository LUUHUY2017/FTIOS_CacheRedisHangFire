using AMMS.ZkAutoPush.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Share.WebApp.Controllers;
using Share.WebApp.Settings;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

IServiceCollection services = builder.Services;
IConfiguration configuration = builder.Configuration;
IConfigurationRoot configRoot = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

services.AddOptions(); //Kích hoạt Options
services.AddVersion();// Versioning

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

AppSettings appSettings = new AppSettings();
configuration.Bind(appSettings);
AuthBaseController.AMMS_Master_HostAddress = builder.Configuration["Authentication:Authority"];

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
        options.Scope.Add("amms.zkteco");


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
    options.AddPolicy("Bearer", policy => policy.RequireClaim("scope", "amms.zkteco"));
});


services.AddApplicationServices();

services.AddDbContext(configuration);

services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

services.AddDistributedMemoryCache();
services.AddSingleton<ISessionStore, DistributedSessionStore>();

// Add services to the container.

services.AddSwaggerGen();
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(
              "",
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
                Scopes = new Dictionary<string, string> { { "amms.zkteco", "AMMS Zkteco API" } }
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
            new List<string>{ "amms.zkteco" }
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

services.AddControllers(); 


services.AddHttpClient();
services.AddControllersWithViews();
services.AddEndpointsApiExplorer();

services.AddEventBusService(configuration);
services.AddCaheService(configuration);
//Các Repo tương tác dữ liệu
services.AddScopedServices();

var app = builder.Build();

app.UseCors("CorsPolicy");

app.UseSession();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseSwaggerUI(c =>
    //{
    //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Zkteco AutoPush Api v1");
    //});

}
// Configure the HTTP request pipeline.

//app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();
app.MapDefaultControllerRoute();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
    )
    .RequireAuthorization()
    ;

});
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
