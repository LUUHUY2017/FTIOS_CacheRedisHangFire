using AMMS.DeviceData.Data;
using AMMS.DeviceData.RabbitMq;
using AMMS.ZkAutoPush.Applications;
using AMMS.ZkAutoPush.Applications.CronJobs;
using AMMS.ZkAutoPush.Applications.V1;
using AMMS.ZkAutoPush.Applications.V1.Consummer;
using AMMS.ZkAutoPush.Datas.Databases;
using EventBus.Messages;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Shared.Core.Caches.Redis;
using Shared.Core.Repositories;
using Shared.Core.SignalRs;
using System.Reflection;

namespace AMMS.ZkAutoPush.Extensions;

public static class DependencyInjection
{
    public static void AddVersion(this IServiceCollection service)
    {
        service.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
        });
        service.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;

        });
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {

        services.AddMediatR(Assembly.GetExecutingAssembly());
        //ConJob chạy các dịch vụ tự động
        services.AddScoped<ICronJobService, CronJobService>();

        return services;
    }


    public static IServiceCollection AddDbContext(this IServiceCollection services,
      IConfiguration configuration)
    {
        #region 
        var MasterDBConnectionType = configuration.GetConnectionString("DefaultConnectionType");
        if (MasterDBConnectionType == "MySQL")
        {
            string mySqlConntectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DeviceAutoPushDbContext>(options =>
           options.UseMySql(
                  mySqlConntectionString
                  , ServerVersion.AutoDetect(mySqlConntectionString)
                  , o => o.SchemaBehavior(MySqlSchemaBehavior.Ignore)
                  )
              //.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)
              //.EnableSensitiveDataLogging()
              //.EnableDetailedErrors()
              )
          ;
        }
        else if (MasterDBConnectionType == "PostgreSQL")
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            services.AddDbContext<DeviceAutoPushDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))

                );
        }
        else
        {
            services.AddDbContext<DeviceAutoPushDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            //services.AddDbContext<DeviceAutoPushDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), builder => builder.UseRowNumberForPaging()));
        }
        #endregion



        services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
        services.AddScoped<IDeviceAutoPushDbContext, DeviceAutoPushDbContext>();


        return services;
    }
    public static void AddEventBusService(this IServiceCollection services, IConfiguration configuration)
    {
        var eventBusSettings = configuration.GetSection("EventBusSettings");  // đọc config
        services.Configure<EventBusSettings>(eventBusSettings);
        services.AddScoped<IEventBusAdapter, EventBusAdapter>();


        services.AddScoped<ZK_DEVICE_RPConsummer>();
        services.AddScoped<ZK_SV_PUSHConsummer>();
        services.AddScoped<ZK_TA_DataConsummer>();


        services.AddMassTransit(config =>
        {
            config.AddConsumer<ZK_TA_DataConsummer>();
            config.AddConsumer<ZK_SV_PUSHConsummer>();
            config.AddConsumer<ZK_DEVICE_RPConsummer>();
            //config.AddConsumer<DeviceConsumer>(); 


            config.UsingRabbitMq((ct, cfg) =>
            {


                cfg.Host(configuration["EventBusSettings:HostAddress"]);


                //provide the queue name with consumer settings
                //Data to SV
                cfg.ReceiveEndpoint($"{configuration["DataArea"]}{EventBusConstants.ZK_Auto_Push_D2S}", c =>
                {
                    c.ConfigureConsumer<ZK_TA_DataConsummer>(ct);
                });
                cfg.ReceiveEndpoint($"{configuration["DataArea"]}{EventBusConstants.ZK_Response_Push_D2S}", c =>
                {
                    c.ConfigureConsumer<ZK_DEVICE_RPConsummer>(ct);
                });
                //Request from SV
                cfg.ReceiveEndpoint($"{configuration["DataArea"]}{EventBusConstants.ZK_Server_Push_S2D}", c =>
                {
                    c.ConfigureConsumer<ZK_SV_PUSHConsummer>(ct);
                });





                cfg.ConfigureEndpoints(ct);
            });
        });

        services.AddMassTransitHostedService();
    }
    public static void AddCaheService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ICacheService, CacheService>();
    }
    public static void AddScopedServices(this IServiceCollection service)
    {
        service.AddScoped<ZK_TA_DataService>();
        service.AddScoped<ZK_DEVICE_RPService>();
        service.AddScoped<ZK_SV_PUSHService>();
        service.AddScoped<StartupDataService>();

        //Cache
        service.AddSingleton<ICacheService, CacheService>();
        service.AddScoped<DeviceCacheService>();
        service.AddScoped<DeviceCommandCacheService>();
        service.AddSingleton<SignalRClientService>();


    }
    public static void AddSignalRService(this IServiceCollection service, IConfiguration configuration)
    {
        service.AddSignalR(o =>
        {
            o.EnableDetailedErrors = true;
            o.MaximumReceiveMessageSize = 4 * 1024 * 1024; // 4MB
        });
        service.AddSingleton<ISignalRAdapter, SignalRAdapter>();
        service.AddScoped<ISignalRService, SignalRService>();
        service.AddSingleton<ISignalRClientService, SignalRClientService>();
    }

}
