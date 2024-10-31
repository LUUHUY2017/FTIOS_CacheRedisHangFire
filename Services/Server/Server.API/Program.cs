using AMMS.Notification.Datas;
using Hangfire;
using Hangfire.MySql;
using IdentityModel.Client;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Newtonsoft.Json;
using Serilog;
using Server.API.Helps.Authorizations;
using Server.API.SignalRs;
using Server.Application.CronJobs;
using Server.Application.Extensions;
using Server.Core.Identity.Entities;
using Server.Infrastructure.Datas.MasterData;
using Server.Infrastructure.Identity;
using Share.WebApp.Controllers;
using Share.WebApp.Settings;
using Shared.Core.Commons;
using Shared.Core.Identity;
using Shared.Core.Loggers;
using System.Reflection;
using System.Text;

bool UserIdentityServer4Memory = false;
var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));
IServiceCollection services = builder.Services;
IConfiguration configuration = builder.Configuration;
IConfigurationRoot configRoot = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

//Log.Logger = new LoggerConfiguration()
//                .MinimumLevel.Warning()
//                .WriteTo.Console()
//                .WriteTo.File("log_quyet_.txt")
//                .CreateLogger();
//services.AddSerilog();
//Log.Debug("Log.Debug");
//Log.Information("Log.Information");
//Log.Warning("Log.Warning");
//Log.Fatal("Log.Fatal");
//Log.Error("Log.Error");


services.AddOptions(); // Kích hoạt Options
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

//Event bus - RabitMQ setting
var eventBusSettings = configuration.GetSection("EventBusSettings");

//SignalR
services.AddSignalRService(configuration);

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



//Add Database Service
services.AddDbContext(configuration);

#region  IdentityServer4

services.AddIdentity<ApplicationUser, IdentityRole>(o =>
{
    // configure identity options
    o.Password.RequireDigit = false;
    o.Password.RequireLowercase = false;
    o.Password.RequireUppercase = false;
    o.Password.RequireNonAlphanumeric = false;
    o.Password.RequiredLength = 6;
}) // để cho nó dùng được UserManger và roleManager
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();

services.ConfigureApplicationCookie((options) =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    // options.AccessDeniedPath = "/AccessDenied";
});



var builder1 = services.AddIdentityServer(options =>
{
    //options.Events.RaiseErrorEvents = true;
    //options.Events.RaiseInformationEvents = true;
    //options.Events.RaiseFailureEvents = true;
    //options.Events.RaiseSuccessEvents = true;



    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseSuccessEvents = true;
    options.EmitStaticAudienceClaim = true;
    options.UserInteraction.LoginUrl = "/Account/Login";
    options.UserInteraction.LogoutUrl = "/Account/Logout";
    options.Authentication = new IdentityServer4.Configuration.AuthenticationOptions()
    {
        CookieLifetime = TimeSpan.FromHours(12),
        CookieSlidingExpiration = true
    };

})
 .AddAspNetIdentity<ApplicationUser>()
 .AddProfileService<Server.API.Services.ProfileService>()

#region cấu hình InMemory 
//.AddInMemoryApiResources(Config.ApiResources) // bên folder IdentityServer thêm Config
//.AddInMemoryClients(Config.Clients) // lấy ra các client // .AddInMemoryClients(Configuration.GetSection("IdentityServer:Clients"))
//.AddInMemoryIdentityResources(Config.IdentityResources)
//.AddInMemoryApiScopes(Config.ApiScopes)
#endregion

#region cấu hình In DB 
//this adds the config data from DB (clients, resources, CORS)
.AddConfigurationStore(options =>
{
    var IdentityDBConnectionType = configuration.GetConnectionString("MasterDBConnectionType");
    if (IdentityDBConnectionType == "MySQL")
    {
        var serverVersion = new MySqlServerVersion(new Version(7, 0, 0));
        options.ConfigureDbContext = db =>
            db.UseMySql(
                     configuration.GetConnectionString("MasterDBConnection"),
                     serverVersion,
                     b => b.MigrationsAssembly("Server.Infrastructure")
                    )

            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors()
            ;
    }
    else
    {
        options.ConfigureDbContext = db =>
            db.UseSqlServer(
                    configuration.GetConnectionString("MasterDBConnection")
                    //, sql => sql.MigrationsAssembly(typeof(Program).GetTypeInfo().Assembly.GetName().Name) //Nếu File MigrationsAssembly trên cùng project
                    , sql => sql.MigrationsAssembly("Server.Infrastructure")
                    );
    }



    //options.DefaultSchema = "IdentityServer";
})
//this adds the operational data from DB (codes, tokens, consents)
.AddOperationalStore(options =>
{
    var IdentityDBConnectionType = configuration.GetConnectionString("MasterDBConnectionType");
    if (IdentityDBConnectionType == "MySQL")
    {
        var serverVersion = new MySqlServerVersion(new Version(7, 0, 0));
        options.ConfigureDbContext = db =>
            db.UseMySql(
                     configuration.GetConnectionString("MasterDBConnection"),
                     serverVersion,
                     b => b.MigrationsAssembly("Server.Infrastructure")
                    )

            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors()
            ;
    }
    else
    {
        options.ConfigureDbContext = db =>
            db.UseSqlServer(
                    configuration.GetConnectionString("MasterDBConnection")
                    //, sql => sql.MigrationsAssembly(typeof(Program).GetTypeInfo().Assembly.GetName().Name) //Nếu File MigrationsAssembly trên cùng project
                    , sql => sql.MigrationsAssembly("Server.Infrastructure")
                    );
    }
    // this enables automatic token cleanup. this is optional.
    options.EnableTokenCleanup = true;
    // options.TokenCleanupInterval = 15; // interval in seconds. 15 seconds useful for debugging

    //options.DefaultSchema = "IdentityServer";
})
#endregion
 ;

builder1.AddDeveloperSigningCredential(); //Chạy trên môi trường dev
                                          //builder1.AddSigningCredential(new X509Certificate2(Path.Combine(".", "certs", "IdentityServer4Auth.pfx"))); //Chạy trên môi trường prod

#endregion


services.Configure<Authentication>(configuration.GetSection("Authentication"));

services.AddAuthentication()
 .AddLocalApi("Bearer", option =>
 {
     option.ExpectedScope = "masterapi";
 });
services.AddAuthorization(options =>
{
    options.AddPolicy("Bearer", policy =>  // thêm một cái chính sách
    {
        policy.AddAuthenticationSchemes("Bearer");
        policy.RequireAuthenticatedUser();
    });
});



services.AddRazorPages(options =>
{
    options.Conventions.AddAreaFolderRouteModelConvention("Identity", "/Account/", model =>
    {
        foreach (var selector in model.Selectors)
        {
            var attributeRouteModel = selector.AttributeRouteModel;
            attributeRouteModel.Order = -1;
            attributeRouteModel.Template = attributeRouteModel.Template.Remove(0, "Identity".Length);
        }
    });
});


services.AddSingleton<IDiscoveryCache>(r =>
{
    var factory = r.GetRequiredService<IHttpClientFactory>();
    //return new DiscoveryCache("https://localhost:5001", () => factory.CreateClient());
    return new DiscoveryCache(builder.Configuration["Authentication:Authority"], () => factory.CreateClient());
});

//AutoMapper
services.AddAutoMapper(typeof(Program));
services.AddAddAutoMapperServices();

//Add App services
services.AddAppService();
services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//Các Repo tương tác dữ liệu
services.AddScopedServices();

// Event Bus
services.AddEventBusService(configuration);

//Pagination
services.AddUriPaginationService();

services.AddHttpClient();



// Add services to the container.
builder.Services.AddControllersWithViews();

//Swagger
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
                Scopes = new Dictionary<string, string> { { "deliveryapi", "Devices API" } }
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
            new List<string>{ "deliveryapi" }
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

//Redis
//services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost"));
//services.AddSingleton<IRedisNotificationService, RedisNotificationService>();
// Resolve the Redis notification service


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();


var app = builder.Build();
//Seriglog
//app.UseSerilogRequestLogging();

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
        var conJobService = scope.ServiceProvider.GetRequiredService<ICronJobService>();
       // RecurringJob.AddOrUpdate("Test", () => conJobService.Write(), "*/1 * * * *", TimeZoneInfo.Local);
        //RecurringJob.AddOrUpdate("CheckDeviceOnline" ,() => conJobService.CheckDeviceOnline(), "*/5 * * * *", TimeZoneInfo.Local);
    }
    catch (Exception e)
    {
        Logger.Error(e);
    }
}


app.UseCors("CorsPolicy");
Common.Follder = app.Environment.WebRootPath;

if (configuration["Authentication:Swagger:Active"] == "True")
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        //c.OAuthClientId("brickstream.api.swagger");
        c.OAuthClientId(configuration["Authentication:Swagger:ClientId"]);
        c.SwaggerEndpoint($"/swagger/v1/swagger.json", "AMMS API V1");
        c.SwaggerEndpoint($"/swagger/v2/swagger.json", "AMMS API V2");
        c.RoutePrefix = "swagger";
    });
}

//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseIdentityServer();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();
app.MapDefaultControllerRoute();
app.MapRazorPages();

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
//app.UseHangfireDashboard("/hangfire_dashboard", new DashboardOptions
//{
//    IgnoreAntiforgeryToken = true,
//    Authorization = new[] { new DashboardNoAuthorizationFilter() }
//});
//app.UseHangfireServer();




app.MapHub<AmmsHub>("/ammshub");
using (var scope = app.Services.CreateScope())
{
    try
    {
        //var masterDataDbContext = scope.ServiceProvider.GetRequiredService<MasterDataDbContext>();
        //await masterDataDbContext.Database.MigrateAsync();

        var identityContext = scope.ServiceProvider.GetRequiredService<IdentityContext>();
        await identityContext.Database.MigrateAsync();


        //var notificationDbContext = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();
        //await notificationDbContext.Database.MigrateAsync();


        var signalRClient = scope.ServiceProvider.GetRequiredService<Shared.Core.SignalRs.ISignalRClientService>();
        signalRClient.Init(AuthBaseController.AMMS_Master_HostAddress + "/ammshub");
        signalRClient.Start();

        //var conJobService = scope.ServiceProvider.GetRequiredService<IConJobService>();
        //var scheduleLists = await sendMailRepository.GetAlls(new ScheduleSendEmailModel() { Actived = "1" });

    }
    catch (Exception e)
    {
        Logger.Error(e);
    }
}



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    if (!UserIdentityServer4Memory)
    {
        using (var scope = app.Services.CreateScope())
        {
            //await scope.ServiceProvider.GetRequiredService<MasterDataDbContext>().Database.MigrateAsync();

            var _roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var roles = _roleManager.Roles.ToList();
            if (roles == null)
                roles = new List<IdentityRole>();
            var rolesDefault = RoleConst.IdentityRoleList();
            foreach (var role in rolesDefault)
            {
                var roleEntity = roles.FirstOrDefault(o => o.Id == role.Id);
                if (roleEntity == null)
                    await _roleManager.CreateAsync(role);
                else
                {
                    roleEntity.Name = role.Name;
                    roleEntity.NormalizedName = role.Name.ToUpper();

                    await _roleManager.UpdateAsync(roleEntity);
                }
            }



            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            if (await userManager.FindByNameAsync("nguyencongquyet@gmail.com") == null)
            {
                var user = new ApplicationUser()
                {
                    UserName = "nguyencongquyet@gmail.com",
                    Email = "nguyencongquyet@gmail.com",
                    FirstName = "Quyết",
                    LastName = "Nguyễn Công",
                    EmailConfirmed = true,
                    PhoneNumber = "0904838565",
                    PhoneNumberConfirmed = true,
                };
                var xx = await userManager.CreateAsync(user, "Acs@1234");
                if (xx.Succeeded)
                {
                    var yy = await userManager.AddToRoleAsync(user, "SuperAdmin");
                    var xxz = await userManager.AddToRoleAsync(user, "Admin");
                }
            }


            await scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>().Database.MigrateAsync();
            var configurationDbContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

            await scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.MigrateAsync();
            var persistedGrantDbContext = scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();





            //////////////////////////////////////

            //IdentityResources
            foreach (var identityResource in configurationDbContext.IdentityResources)
            {
                if (SystemConfig.IdentityResources.FirstOrDefault(o => o.Name == identityResource.Name) == null)
                    configurationDbContext.IdentityResources.Remove(identityResource);
            }
            await configurationDbContext.SaveChangesAsync();

            foreach (var identityResource in SystemConfig.IdentityResources)
            {
                if (configurationDbContext.IdentityResources.FirstOrDefault(o => o.Name == identityResource.Name) == null)
                    configurationDbContext.IdentityResources.Add(identityResource.ToEntity());
            }
            await configurationDbContext.SaveChangesAsync();


            //ApiScopes
            foreach (var apiScope in configurationDbContext.ApiScopes.ToList())
            {
                if (SystemConfig.ApiScopes.FirstOrDefault(o => o.Name == apiScope.Name) == null)
                    configurationDbContext.ApiScopes.Remove(apiScope);
            }
            await configurationDbContext.SaveChangesAsync();

            foreach (var apiScope in SystemConfig.ApiScopes)
            {
                if (configurationDbContext.ApiScopes.FirstOrDefault(o => o.Name == apiScope.Name) == null)
                    configurationDbContext.ApiScopes.Add(apiScope.ToEntity());
            }
            await configurationDbContext.SaveChangesAsync();



            //ApiResources
            foreach (var apiResources in configurationDbContext.ApiResources.ToList())
            {
                if (SystemConfig.ApiResources.FirstOrDefault(o => o.Name == apiResources.Name) == null)
                    configurationDbContext.ApiResources.Remove(apiResources);
            }
            await configurationDbContext.SaveChangesAsync();

            foreach (var apiResources in SystemConfig.ApiResources)
            {
                if (configurationDbContext.ApiResources.FirstOrDefault(o => o.Name == apiResources.Name) == null)
                    configurationDbContext.ApiResources.Add(apiResources.ToEntity());
            }
            await configurationDbContext.SaveChangesAsync();


            //Clients
            foreach (var cli in configurationDbContext.Clients)
            {
                //if (Config.Clients.FirstOrDefault(o => o.ClientId == client.ClientId) == null)
                configurationDbContext.Clients.Remove(cli);
            }
            await configurationDbContext.SaveChangesAsync();


            //var clientJson = JsonConvert.SerializeObject(Config.Clients); 
            var clientJson = File.ReadAllText($"{Directory.GetCurrentDirectory()}/clients.json");
            var clients = JsonConvert.DeserializeObject<List<Client>>(clientJson);

            //foreach (var cli in Config.Clients)
            foreach (var cli in clients)
            {
                if (configurationDbContext.Clients.FirstOrDefault(o => o.ClientId == cli.ClientId) == null)
                    configurationDbContext.Clients.Add(cli.ToEntity());
            }
            await configurationDbContext.SaveChangesAsync();


        }
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