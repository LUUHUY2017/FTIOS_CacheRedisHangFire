using AMMS.DeviceData.RabbitMq;
using AMMS.VIETTEL.SMAS.Applications.CronJobs;
using AMMS.VIETTEL.SMAS.Applications.Services.AccountVTSmarts.V1;
using AMMS.VIETTEL.SMAS.Applications.Services.AppConfigs.V1;
using AMMS.VIETTEL.SMAS.Applications.Services.Organizations.V1;
using AMMS.VIETTEL.SMAS.Applications.Services.SchoolYearClasses;
using AMMS.VIETTEL.SMAS.Applications.Services.Students.V1;
using AMMS.VIETTEL.SMAS.Applications.Services.TimeAttendenceSyncs;
using AMMS.VIETTEL.SMAS.Applications.Services.TimeAttendenceSyncs.V1;
using AMMS.VIETTEL.SMAS.Applications.Services.TimeConfigs.V1;
using AMMS.VIETTEL.SMAS.Applications.Services.VTSmart;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.AppConfigs;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.ClassRooms;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.Organizations;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.Persons;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.ScheduleJobs;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.SchoolYears;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.StudentClassRoomYears;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.Students;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.TimeConfigs;
using AMMS.VIETTEL.SMAS.Infratructures.Databases;
using AMMS.VIETTEL.SMAS.Infratructures.Identity;
using AMMS.VIETTEL.SMAS.Infratructures.Repositories.AppConfigs;
using AMMS.VIETTEL.SMAS.Infratructures.Repositories.ClassRooms;
using AMMS.VIETTEL.SMAS.Infratructures.Repositories.Organizations;
using AMMS.VIETTEL.SMAS.Infratructures.Repositories.Persons;
using AMMS.VIETTEL.SMAS.Infratructures.Repositories.ScheduleJobs;
using AMMS.VIETTEL.SMAS.Infratructures.Repositories.SchoolYears;
using AMMS.VIETTEL.SMAS.Infratructures.Repositories.StudentClassRoomYears;
using AMMS.VIETTEL.SMAS.Infratructures.Repositories.Students;
using AMMS.VIETTEL.SMAS.Infratructures.Repositories.TimeConfigs;
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

namespace AMMS.VIETTEL.SMAS.Applications.Extensions;

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
        //Oranization
        service.AddScoped<IOrganizationRepository, OrganizationRepository>();
        service.AddScoped<OrganizationService>();

        //TimeConfig
        service.AddScoped<ITimeConfigRepository, TimeConfigRepository>();
        service.AddScoped<TimeConfigService>();

        //AppConfig
        service.AddScoped<IAppConfigRepository, AppConfigRepository>();
        service.AddScoped<AppConfigService>();

        //VTSmart
        service.AddScoped<SmartService>();
        service.AddScoped<AccountVTSmartService>();

        // Person
        service.AddScoped<IPersonRepository, PersonRepository>();
        //service.AddScoped<SyncDeviceServerService>();

        //Cache
        service.AddScoped<ICacheService, CacheService>();

        // Students
        service.AddScoped<IStudentRepository, StudentRepository>();
        service.AddScoped<IClassRoomRepository, ClassRoomRepository>();
        service.AddScoped<ISchoolYearRepository, SchoolYearRepository>();
        service.AddScoped<IStudentClassRoomYearRepository, StudentClassRoomYearRepository>();
        service.AddScoped<StudentService>();
        service.AddScoped<SchoolYearClassService>();

        //ScheduleJob
        service.AddScoped<IScheduleJobRepository, ScheduleJobRepository>();
        service.AddScoped<IScheduleJobLogRepository, ScheduleJobLogRepository>();

        ////  TimeAttendenceEvents
        service.AddScoped<TimeAttendenceSyncService>();

        //service.AddScoped<ITATimeAttendenceEventRepository, TATimeAttendenceEventRepository>();
        //service.AddScoped<ITATimeAttendenceDetailRepository, TATimeAttendenceDetailRepository>();
        //service.AddScoped<ITATimeAttendenceSyncRepository, TATimeAttendenceSyncRepository>();


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
    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IdentityContext>(options =>
                options.UseMySql(
                        configuration.GetConnectionString("DefaultConnection")
                        , ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection"))

                        //, o => o.SchemaBehavior(MySqlSchemaBehavior.Ignore) 
                        , o => o.SchemaBehavior(MySqlSchemaBehavior.Translate, (schema, table) => $"{schema}.{table}")
                        )
                    //.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)
                    //.EnableSensitiveDataLogging()
                    //.EnableDetailedErrors()
                    , ServiceLifetime.Scoped
                    ); services.AddDbContext<IdentityContext>(options =>
                options.UseMySql(
                        configuration.GetConnectionString("DefaultConnection")
                        , ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection"))

                        //, o => o.SchemaBehavior(MySqlSchemaBehavior.Ignore) 
                        , o => o.SchemaBehavior(MySqlSchemaBehavior.Translate, (schema, table) => $"{schema}.{table}")
                        )
                    //.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)
                    //.EnableSensitiveDataLogging()
                    //.EnableDetailedErrors()
                    , ServiceLifetime.Scoped
                    );

        #region 
        var MasterDBConnectionType = configuration.GetConnectionString("DefaultConnectionType");
        if (MasterDBConnectionType == "MySQL")
        {
            string mySqlConntectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ViettelDbContext>(options =>
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
            services.AddDbContext<ViettelDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))

                );
        }
        else
        {
            services.AddDbContext<ViettelDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            //services.AddDbContext<DeviceAutoPushDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), builder => builder.UseRowNumberForPaging()));
        }
        #endregion



        services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
        services.AddScoped<IViettelDbContext, ViettelDbContext>();


        return services;
    }

    public static void AddEventBusService(this IServiceCollection services, IConfiguration configuration)
    {
        ;
        // Đọc Config AppSetting
        var eventBusSettings = configuration.GetSection("EventBusSettings");
        // Đăng ký EventBusAdapter
        services.AddScoped<IEventBusAdapter, EventBusAdapter>();
        // Kết nối RabbitMQ
        services.Configure<EventBusSettings>(eventBusSettings);
        // Đăng ký dịch vụ SyncDevice&Ta
        services.AddScoped<TimeAttendenceSyncServiceConsumer>();

        services.AddMassTransit(config =>
        {
            ////Đăng ký xử lý bản tin 
            config.AddConsumer<TimeAttendenceSyncServiceConsumer>();

            config.UsingRabbitMq((ct, cfg) =>
            {
                cfg.Host(configuration["EventBusSettings:HostAddress"]);

                #region  Nhận lệnh đồng bộ điểm danh sang Smas 1 học sinh từ MasterData
                //// Nhận Response từ Sự kiện đồng bộ thiết bị trả về
                cfg.ReceiveEndpoint($"{configuration["DataArea"]}{EventBusConstants.Server_Auto_Push_SMAS}", c =>
                {
                    c.ConfigureConsumer<TimeAttendenceSyncServiceConsumer>(ct);
                });
                #endregion
                cfg.ConfigureEndpoints(ct);
            });
            services.AddMassTransitHostedService();
        });
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
