using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Share.WebApp.Controllers;
using Share.WebApp.Settings;

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
        options.Scope.Add("amms.hanet");


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
    options.AddPolicy("Bearer", policy => policy.RequireClaim("scope", "amms.hanet"));
});

// Add services to the container.

builder.Services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
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
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hanet Api v1");
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
