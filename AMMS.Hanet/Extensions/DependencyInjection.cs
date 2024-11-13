using AMMS.DeviceData.RabbitMq;
using AMMS.Hanet.Applications;
using AMMS.Hanet.Applications.AppConfigs.V1;
using AMMS.Hanet.Applications.CronJobs;
using AMMS.Hanet.Applications.MonitorDevices.V1;
using AMMS.Hanet.Applications.TerminalCommandLogs.V1;
using AMMS.Hanet.Applications.V1.Consummer;
using AMMS.Hanet.Applications.V1.Service;
using AMMS.Hanet.Datas.Databases;
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

namespace AMMS.Hanet.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddAddAutoMapperServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        return services;
    }
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

    public static void AddScopedServices(this IServiceCollection service)
    {
        //Device
        //service.AddScoped<HANET_Process_Service>();

        // AppConfig
        service.AddScoped<AppConfigService>();
        service.AddScoped<HANET_Server_Push_Service>();
        service.AddScoped<HANET_Device_Reponse_Service>();
        service.AddScoped<HANET_StartUp_Service>();
        service.AddScoped<HANET_API_Service>();

        // Monitor Device
        service.AddScoped<MonitorDeviceService>();
        service.AddScoped<CacheService>();
        service.AddScoped<DeviceCacheService>();
        service.AddSingleton<SignalRClientService>();
        //service.AddScoped<CronJobService>();

        // TerminalCommandLog
        service.AddScoped<TerminalCommandLogService>();

    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        //services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddMediatR(Assembly.GetExecutingAssembly());
        //services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

        //services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));

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


            services.AddDbContext<ViettelDbContext>(options =>
            options.UseMySql(
                  mySqlConntectionString
                  , ServerVersion.AutoDetect(mySqlConntectionString)
                  , o => o.SchemaBehavior(MySqlSchemaBehavior.Ignore)
                  ) 
              )
            ;
        }
        else if (MasterDBConnectionType == "PostgreSQL")
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            services.AddDbContext<DeviceAutoPushDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))

                );
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            services.AddDbContext<ViettelDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))

                );
        }
        else
        {
            services.AddDbContext<DeviceAutoPushDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            //services.AddDbContext<DeviceAutoPushDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), builder => builder.UseRowNumberForPaging()));

            services.AddDbContext<ViettelDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        }
        #endregion



        services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
        services.AddScoped<IDeviceAutoPushDbContext, DeviceAutoPushDbContext>();

        services.AddScoped<IViettelDbContext, ViettelDbContext>();

        return services;
    }

    public static void AddEventBusService(this IServiceCollection services, IConfiguration configuration)
    {
        var eventBusSettings = configuration.GetSection("EventBusSettings");  // đọc config
        services.Configure<EventBusSettings>(eventBusSettings);
        services.AddScoped<IEventBusAdapter, EventBusAdapter>();


        services.AddScoped<HANET_Checkin_DataConsummer>();
        services.AddScoped<HANET_DEVICE_PUSHConsummer>();
        services.AddScoped<HANET_SERVER_PUSHConsummer>();


        services.AddMassTransit(config =>
        {
            config.AddConsumer<HANET_Checkin_DataConsummer>();
            config.AddConsumer<HANET_DEVICE_PUSHConsummer>();
            config.AddConsumer<HANET_SERVER_PUSHConsummer>();


            config.UsingRabbitMq((ct, cfg) =>
            {
                //cfg.Host(configuration["EventBusSettings:HostAddress"], h =>
                //{
                //    h.Username("guest");
                //    h.Password("guest");
                //});

                cfg.Host(configuration["EventBusSettings:HostAddress"]);


                //provide the queue name with consumer settings
                cfg.ReceiveEndpoint($"{configuration["DataArea"]}_{EventBusConstants.HANET}{EventBusConstants.Hanet_Auto_Push_D2S}", c =>
                {
                    c.ConfigureConsumer<HANET_Checkin_DataConsummer>(ct);
                });
                cfg.ReceiveEndpoint($"{configuration["DataArea"]}_{EventBusConstants.HANET}{EventBusConstants.Hanet_Device_Push_D2S}", c =>
                {
                    c.ConfigureConsumer<HANET_DEVICE_PUSHConsummer>(ct);
                });
                cfg.ReceiveEndpoint($"{configuration["DataArea"]}_{EventBusConstants.HANET}{EventBusConstants.Hanet_Server_Push_S2D}", c =>
                {
                    c.ConfigureConsumer<HANET_SERVER_PUSHConsummer>(ct);
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
