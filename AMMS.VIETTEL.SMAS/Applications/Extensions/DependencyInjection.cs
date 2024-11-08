﻿using AMMS.VIETTEL.SMAS.Applications.CronJobs; using AMMS.VIETTEL.SMAS.Applications.Services.AccountVTSmarts.V1; using AMMS.VIETTEL.SMAS.Applications.Services.AppConfigs.V1; using AMMS.VIETTEL.SMAS.Applications.Services.Organizations.V1; using AMMS.VIETTEL.SMAS.Applications.Services.Students.V1; using AMMS.VIETTEL.SMAS.Applications.Services.TimeConfigs.V1; using AMMS.VIETTEL.SMAS.Cores.Interfaces; using AMMS.VIETTEL.SMAS.Cores.Interfaces.AppConfigs; using AMMS.VIETTEL.SMAS.Cores.Interfaces.Organizations; using AMMS.VIETTEL.SMAS.Cores.Interfaces.Persons; using AMMS.VIETTEL.SMAS.Cores.Interfaces.ScheduleJobs; using AMMS.VIETTEL.SMAS.Cores.Interfaces.SendEmails; using AMMS.VIETTEL.SMAS.Cores.Interfaces.Students; using AMMS.VIETTEL.SMAS.Cores.Interfaces.TimeConfigs; using AMMS.VIETTEL.SMAS.Infratructures.Databases; using AMMS.VIETTEL.SMAS.Infratructures.Identity; using AMMS.VIETTEL.SMAS.Infratructures.Repositories; using AMMS.VIETTEL.SMAS.Infratructures.Repositories.AppConfigs; using AMMS.VIETTEL.SMAS.Infratructures.Repositories.Organizations; using AMMS.VIETTEL.SMAS.Infratructures.Repositories.Persons; using AMMS.VIETTEL.SMAS.Infratructures.Repositories.ScheduleJobs; using AMMS.VIETTEL.SMAS.Infratructures.Repositories.SendEmails; using AMMS.VIETTEL.SMAS.Infratructures.Repositories.Students; using MassTransit; using MediatR; using Microsoft.AspNetCore.Mvc; using Microsoft.EntityFrameworkCore; using Pomelo.EntityFrameworkCore.MySql.Infrastructure; using Server.Application.Services.VTSmart; using Shared.Core.Caches.Redis; using Shared.Core.Repositories; using System.Reflection;  namespace AMMS.VIETTEL.SMAS.Applications.Extensions;  public static class DependencyInjection {     public static IServiceCollection AddAddAutoMapperServices(this IServiceCollection services)     {         services.AddAutoMapper(Assembly.GetExecutingAssembly());         return services;     }     public static void AddVersion(this IServiceCollection service)     {         service.AddApiVersioning(options =>         {             options.AssumeDefaultVersionWhenUnspecified = true;             options.DefaultApiVersion = new ApiVersion(1, 0);             options.ReportApiVersions = true;         });         service.AddVersionedApiExplorer(options =>         {             options.GroupNameFormat = "'v'VVV";             options.SubstituteApiVersionInUrl = true;          });     }      public static void AddScopedServices(this IServiceCollection service)     {         //Oranization         service.AddScoped<IOrganizationRepository, OrganizationRepository>();         service.AddScoped<OrganizationService>();          //TimeConfig         service.AddScoped<ITimeConfigRepository, TimeConfigRepository>();         service.AddScoped<TimeConfigService>();          //AppConfig         service.AddScoped<IAppConfigRepository, AppConfigRepository>();         service.AddScoped<AppConfigService>();          //VTSmart         service.AddScoped<SmartService>();          //AccountVTSmart         service.AddScoped<AccountVTSmartService>();          //////////////////////////         ///// ScheduleSendMail         service.AddScoped<IScheduleSendEmailDetailRepository, ScheduleSendEmailDetailRepository>();         service.AddScoped<IScheduleSendMailRepository, ScheduleSendEmailRepository>();           // Person         service.AddScoped<IPersonRepository, PersonRepository>();         //service.AddScoped<SyncDeviceServerService>();          //SendEmail         service.AddScoped<ISendEmailRepository, SendEmailRepository>();         service.AddScoped<ISendEmailLogRepository, SendEmailLogRepository>();         service.AddScoped<IScheduleJobRepository, ScheduleJobRepository>();          //Cache         service.AddScoped<ICacheService, CacheService>();          // Students         service.AddScoped<IStudentRepository, StudentRepository>();         service.AddScoped<StudentService>();           ////  TimeAttendenceEvents         //service.AddScoped<ITATimeAttendenceEventRepository, TATimeAttendenceEventRepository>();         //service.AddScoped<ITATimeAttendenceDetailRepository, TATimeAttendenceDetailRepository>();         //service.AddScoped<ITATimeAttendenceSyncRepository, TATimeAttendenceSyncRepository>();         //service.AddScoped<TimeAttendenceSyncService>();           //// AMMS. Notification -  SendEmail         //service.AddScoped<INSendEmailRepository, NSendEmailRepository>();         //service.AddScoped<INSendEmailLogRepository, NSendEmailLogRepository>();          //service.AddScoped<IAccountService, AccountService>();       }      public static IServiceCollection AddApplicationServices(this IServiceCollection services)     {         //services.AddAutoMapper(Assembly.GetExecutingAssembly());          services.AddMediatR(Assembly.GetExecutingAssembly());         //services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));          //services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());         //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));         //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));          //ConJob chạy các dịch vụ tự động         services.AddScoped<ICronJobService, CronJobService>();         return services;     }      public static IServiceCollection AddDbContext(this IServiceCollection services,       IConfiguration configuration)     {          #region          var SmasDBConnectionType = configuration.GetConnectionString("DefaultConnectionType");         if (SmasDBConnectionType == "MySQL")         {             string mySqlConntectionString = configuration.GetConnectionString("DefaultConnection");             services.AddDbContext<ViettelDbContext>(options =>            options.UseMySql(                   mySqlConntectionString                   , ServerVersion.AutoDetect(mySqlConntectionString)                   , o => o.SchemaBehavior(MySqlSchemaBehavior.Ignore)                   )               //.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)               //.EnableSensitiveDataLogging()               //.EnableDetailedErrors()               )           ;         }         else if (SmasDBConnectionType == "PostgreSQL")         {             AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);             services.AddDbContext<ViettelDbContext>(options =>                 options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))                  );         }         else         {             services.AddDbContext<ViettelDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));             //services.AddDbContext<DeviceAutoPushDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), builder => builder.UseRowNumberForPaging()));         }         #endregion                    services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));         services.AddScoped<IViettelDbContext, ViettelDbContext>();                     #region DefaultConnectionType         var MasterDBConnectionType = configuration.GetConnectionString("DefaultConnectionType");         if (MasterDBConnectionType == "MySQL")         {             services.AddDbContext<MasterDataDbContext>(options =>                options.UseMySql(                       configuration.GetConnectionString("DefaultConnectionType")                       , ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnectionType"))                       , o => o.SchemaBehavior(MySqlSchemaBehavior.Ignore)                       )                   //.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)                   //.EnableSensitiveDataLogging()                   //.EnableDetailedErrors()                   )               ;         }         else if (MasterDBConnectionType == "PostgreSQL")         {             AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);             services.AddDbContext<MasterDataDbContext>(options =>                 options.UseNpgsql(configuration.GetConnectionString("DefaultConnectionType"))                  );         }         else         {             //services.AddDbContext<MasterDataDbContext>(options => options.UseSqlServer(             //   configuration.GetConnectionString("DefaultConnectionType")));              services.AddDbContext<MasterDataDbContext>(options => options.UseSqlServer(               configuration.GetConnectionString("DefaultConnectionType")));         }
        #endregion          services.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBaseMasterData<>));         services.AddScoped<IMasterDataDbContext, MasterDataDbContext>();          return services;     }      public static void AddEventBusService(this IServiceCollection services, IConfiguration configuration)     {         var eventBusSettings = configuration.GetSection("EventBusSettings");  // đọc config                                                                               //services.Configure<EventBusSettings>(eventBusSettings);                                                                               //services.AddScoped<IEventBusAdapter, EventBusAdapter>();            services.AddMassTransit(config =>         {               config.UsingRabbitMq((ct, cfg) =>             {                 //cfg.Host(configuration["EventBusSettings:HostAddress"], h =>                 //{                 //    h.Username("guest");                 //    h.Password("guest");                 //});                  cfg.Host(configuration["EventBusSettings:HostAddress"]);                   //provide the queue name with consumer settings                 //cfg.ReceiveEndpoint($"{configuration["DataArea"]}{EventBusConstants.Hanet_Auto_Push_D2S}", c =>                 //{                 //    c.ConfigureConsumer<HANET_Checkin_DataConsummer>(ct);                 //});                   cfg.ConfigureEndpoints(ct);             });         });          services.AddMassTransitHostedService();     }     public static void AddCaheService(this IServiceCollection services, IConfiguration configuration)     {         services.AddSingleton<ICacheService, CacheService>();     }  } 