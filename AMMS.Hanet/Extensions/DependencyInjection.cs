using AMMS.DeviceData.RabbitMq;
using AMMS.Hanet.Applications.AppConfigs.V1;
using AMMS.Hanet.Datas.Databases;
using EventBus.Messages;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Shared.Core.Caches.Redis;
using Shared.Core.Repositories;
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
        // AppConfig
        service.AddScoped<AppConfigService>();
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        //services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddMediatR(Assembly.GetExecutingAssembly());
        //services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));

        //services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));

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


        //services.AddScoped<BrickstreamConsumer>(); 
        //services.AddScoped<DeviceConsumer>(); 


        services.AddMassTransit(config =>
        {
            //config.AddConsumer<BrickstreamConsumer>();
            //config.AddConsumer<DeviceConsumer>(); 


            config.UsingRabbitMq((ct, cfg) =>
            {
                //cfg.Host(configuration["EventBusSettings:HostAddress"], h =>
                //{
                //    h.Username("guest");
                //    h.Password("guest");
                //});

                cfg.Host(configuration["EventBusSettings:HostAddress"]);


                //provide the queue name with consumer settings
                cfg.ReceiveEndpoint($"{configuration["DataArea"]}{EventBusConstants.ZK_Auto_Push_D2S}", c =>
                {
                    //c.ConfigureConsumer<BrickstreamConsumer>(ct);
                });
                //cfg.ReceiveEndpoint($"{configuration["DataArea"]}{EventBusConstants.BrickstreamData}", c =>
                //{
                //    c.ConfigureConsumer<PeopleCounttingConsumer>(ct);
                //});

                //cfg.ReceiveEndpoint($"{configuration["DataArea"]}{EventBusConstants.DeviceOnlineOffline_Queue}", c =>
                //{
                //    c.ConfigureConsumer<DeviceConsumer>(ct);
                //});


                cfg.ConfigureEndpoints(ct);
            });
        });

        services.AddMassTransitHostedService();
    }
    public static void AddCaheService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ICacheService, CacheService>();
    }

}
