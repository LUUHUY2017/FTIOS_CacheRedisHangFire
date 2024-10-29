using AMMS.DeviceData.Data;
using AMMS.DeviceData.RabbitMq;
using AMMS.Notification.Datas;
using AMMS.Notification.Datas.Interfaces.SendEmails;
using AMMS.Notification.Datas.Repositories.SendEmails;
using EntityFrameworkCore.UseRowNumberForPaging;
using EventBus.Messages;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Server.Application.MasterDatas.A0.Accounts.V1;
using Server.Application.MasterDatas.A0.AttendanceConfigs.V1;
using Server.Application.MasterDatas.A0.TimeConfigs.V1;
using Server.Application.MasterDatas.A2.Devices;
using Server.Application.MasterDatas.A2.Organizations.V1;
using Server.Application.MasterDatas.A2.Students.V1;
using Server.Application.MasterDatas.TA.TimeAttendenceEvents.V1;
using Server.Application.Services.VTSmart;
using Server.Core.Identity.Interfaces.Accounts.Services;
using Server.Core.Interfaces.A0;
using Server.Core.Interfaces.A2.Devices;
using Server.Core.Interfaces.A2.Organizations;
using Server.Core.Interfaces.A2.Persons;
using Server.Core.Interfaces.A2.ScheduleSendEmails;
using Server.Core.Interfaces.A2.SendEmails;
using Server.Core.Interfaces.A2.Students;
using Server.Core.Interfaces.GIO.VehicleInOuts;
using Server.Core.Interfaces.TA.TimeAttendenceDetails;
using Server.Core.Interfaces.TA.TimeAttendenceEvents;
using Server.Infrastructure.Datas.MasterData;
using Server.Infrastructure.Identity;
using Server.Infrastructure.Repositories.A0.AttendanceConfigs;
using Server.Infrastructure.Repositories.A0.TimeConfigs;
using Server.Infrastructure.Repositories.A2.Devices;
using Server.Infrastructure.Repositories.A2.Organizations;
using Server.Infrastructure.Repositories.A2.Persons;
using Server.Infrastructure.Repositories.A2.ScheduleSendEmails;
using Server.Infrastructure.Repositories.A2.SendEmails;
using Server.Infrastructure.Repositories.A2.Students;
using Server.Infrastructure.Repositories.GIO.TimeAttendenceDetails;
using Server.Infrastructure.Repositories.GIO.VehicleInOuts;
using Share.Core.Pagination;
using Shared.Core.Caches.Redis;
using Shared.Core.Emails.V1.Adapters;
using Shared.Core.Repositories;
using Shared.Core.SignalRs;
using System.Reflection;

namespace Server.Application.Extensions;

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

    public static void AddEventBusService(this IServiceCollection services, IConfiguration configuration)
    {
        // Đọc Config AppSetting
        var eventBusSettings = configuration.GetSection("EventBusSettings");
        // Đăng ký EventBusAdapter
        services.AddScoped<IEventBusAdapter, EventBusAdapter>();
        // Kết nối RabbitMQ
        services.Configure<EventBusSettings>(eventBusSettings);

        // Đăng ký dịch vụ Email
        //services.AddScoped<SendEmailMessageService1>();
        //services.AddScoped<EmailSenderServiceV1>();
        //services.AddScoped<EmailRabbitMQConsummerV1>();
        //services.AddScoped<SendEmailMessageResponseConsumer1>();

        // Đăng ký dịch vụ SyncDevice&Ta
        services.AddScoped<TimeAttendenceEventService>();
        services.AddScoped<StudentConsumer>();


        services.AddScoped<TimeAttendenceEventService>();
        services.AddScoped<TimeAttendenceEventConsumer>();



        services.AddMassTransit(config =>
        {
            // Đăng ký dịch vụ nghe vào Rabbbit
            //config.AddConsumer<EmailRabbitMQConsummerV1>();
            //config.AddConsumer<SendEmailMessageResponseConsumer1>();


            ////Đăng ký xử lý bản tin data XML của Brickstream
            config.AddConsumer<StudentConsumer>();
            config.AddConsumer<TimeAttendenceEventConsumer>();
            //Đăng ký xử lý bản tin xuống thiết bị
            config.AddConsumer<Server_RequestConsummer>();

            config.UsingRabbitMq((ct, cfg) =>
            {
                //cfg.Host(configuration["EventBusSettings:HostAddress"], h =>
                //{
                //    h.Username("guest");
                //    h.Password("guest");
                //});

                cfg.Host(configuration["EventBusSettings:HostAddress"]);

                #region  Device
                //// Nhận Response từ Sự kiện đồng bộ thiết bị trả về
                cfg.ReceiveEndpoint($"{EventBusConstants.DataArea}{EventBusConstants.Device_Auto_Push_D2S}", c =>
                //cfg.ReceiveEndpoint($"{EventBusConstants.DataArea}{EventBusConstants.Server_Auto_Push_S2D}", c =>
                {
                    c.ConfigureConsumer<StudentConsumer>(ct);
                });
                #endregion

                #region  Report
                //// Nhận Response từ Sự kiện đồng bộ thiết bị trả về
                cfg.ReceiveEndpoint($"{EventBusConstants.DataArea}{EventBusConstants.Data_Auto_Push_D2S}", c =>
                {
                    c.ConfigureConsumer<TimeAttendenceEventConsumer>(ct);
                });
                #endregion

                #region Gửi request xuống máy trạm
                //Request from SV
                cfg.ReceiveEndpoint($"{EventBusConstants.DataArea}{EventBusConstants.Server_Auto_Push_S2D}", c =>
                {
                    c.ConfigureConsumer<Server_RequestConsummer>(ct);
                });
                #endregion



                #region Email 
                ////Email Sending
                //cfg.ReceiveEndpoint($"{configuration["DataArea"]}{EmailConst.EventBusChanelSendEmail}", c =>
                //{
                //    c.ConfigureConsumer<EmailRabbitMQConsummerV1>(ct);
                //});
                ////Email Receiving
                //cfg.ReceiveEndpoint($"{configuration["DataArea"]}{EmailConst.EventBusChanelSendEmailResponse}", c =>
                //{
                //    c.ConfigureConsumer<SendEmailMessageResponseConsumer1>(ct);
                //});
                #endregion



                cfg.ConfigureEndpoints(ct);
            });
        });

        services.AddMassTransitHostedService();
    }

    public static void AddAppService(this IServiceCollection service)
    {
        service.AddMediatR(Assembly.GetExecutingAssembly());

        //MasterData
        service.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBaseMasterData<>));
        service.AddScoped<IMasterDataDbContext, MasterDataDbContext>();


        //Cache
        service.AddSingleton<ICacheService, CacheService>();

        ////ConJob chạy các dịch vụ tự động
        //service.AddScoped<IConJobService, ConJobService>();

        service.AddScoped<SMTP_Email_Adpater>();
        service.AddTransient<Microsoft.AspNetCore.Identity.UI.Services.IEmailSender, EmailSenderService>();

    }

    public static void AddScopedServices(this IServiceCollection service)
    {
        // ScheduleSendMail
        service.AddScoped<IScheduleSendEmailDetailRepository, ScheduleSendEmailDetailRepository>();
        service.AddScoped<IScheduleSendMailRepository, ScheduleSendEmailRepository>();


        // Person
        service.AddScoped<IPersonRepository, PersonRepository>();
        service.AddScoped<SyncDeviceServerService>();

        //SendEmail
        service.AddScoped<ISendEmailRepository, SendEmailRepository>();
        service.AddScoped<ISendEmailLogRepository, SendEmailLogRepository>();

        //Cache
        service.AddScoped<ICacheService, CacheService>();

        //Device
        service.AddScoped<IDeviceRepository, DeviceRepository>();
        service.AddScoped<DeviceService>();
        service.AddScoped<DeviceAdminService>();

        //Lane
        service.AddScoped<ILaneRepository, LaneRepository>();

        // Students
        service.AddScoped<IStudentRepository, StudentRepository>();
        service.AddScoped<StudentService>();


        //  TimeAttendenceEvents
        service.AddScoped<ITATimeAttendenceEventRepository, TATimeAttendenceEventRepository>();
        service.AddScoped<ITATimeAttendenceDetailRepository, TATimeAttendenceDetailRepository>();


        // AMMS. Notification -  SendEmail
        service.AddScoped<INSendEmailRepository, NSendEmailRepository>();
        service.AddScoped<INSendEmailLogRepository, NSendEmailLogRepository>();

        service.AddScoped<IAccountService, AccountService>();

        // GIO_VehicleInOut
        service.AddScoped<IGIOVehicleInOutRepository, GIOVehicleInOutRepository>();


        // AttendanceConfig
        service.AddScoped<IAttendanceConfigRepository, AttendanceConfigRepository>();
        service.AddScoped<AttendanceConfigService>();

        //Organization
        service.AddScoped<IOrganizationRepository, OrganizationRepository>();
        service.AddScoped<OrganizationService>();

        //TimeConfig
        service.AddScoped<ITimeConfigRepository, TimeConfigRepository>();
        service.AddScoped<TimeConfigService>();

        // Đồng bộ dữ liệu
        service.AddScoped<SmartService>();


    }

    public static IServiceCollection AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        #region IdentityContext
        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<IdentityContext>(options =>
                options.UseInMemoryDatabase("IdentityDb"));
        }
        else
        {
            var IdentityDBConnectionType = configuration.GetConnectionString("MasterDBConnectionType");
            if (IdentityDBConnectionType == "MySQL")
            {
                services.AddDbContext<IdentityContext>(options =>
                options.UseMySql(
                        configuration.GetConnectionString("MasterDBConnection")
                        , ServerVersion.AutoDetect(configuration.GetConnectionString("MasterDBConnection"))

                        //, o => o.SchemaBehavior(MySqlSchemaBehavior.Ignore) 
                        , o => o.SchemaBehavior(MySqlSchemaBehavior.Translate, (schema, table) => $"{schema}.{table}")
                        )
                    //.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)
                    //.EnableSensitiveDataLogging()
                    //.EnableDetailedErrors()
                    , ServiceLifetime.Scoped
                    );

            }
            else if (IdentityDBConnectionType == "PostgreSQL")
            {
                AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
                services.AddDbContext<IdentityContext>(options => options.UseNpgsql(configuration.GetConnectionString("MasterDBConnection")));
            }
            else
            {
                services.AddDbContext<IdentityContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("MasterDBConnection"), b => b.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName)));
            }
        }
        #endregion

        #region MasterDBConnection
        var MasterDBConnectionType = configuration.GetConnectionString("MasterDBConnectionType");
        if (MasterDBConnectionType == "MySQL")
        {
            services.AddDbContext<MasterDataDbContext>(options =>
               options.UseMySql(
                      configuration.GetConnectionString("MasterDBConnection")
                      , ServerVersion.AutoDetect(configuration.GetConnectionString("MasterDBConnection"))
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
            services.AddDbContext<MasterDataDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("MasterDBConnection"))

                );
        }
        else
        {
            //services.AddDbContext<MasterDataDbContext>(options => options.UseSqlServer(
            //   configuration.GetConnectionString("MasterDBConnection")));

            services.AddDbContext<MasterDataDbContext>(options => options.UseSqlServer(
              configuration.GetConnectionString("MasterDBConnection"), builder => builder.UseRowNumberForPaging()));
        }
        #endregion

        //var xx = configuration.GetConnectionString("MasterDBConnection");
        //services.AddDbContext<BiDbContext>(options => options.UseSqlServer(xx, builder => builder.UseRowNumberForPaging()));

        var xxx = configuration.GetConnectionString("MasterDBConnection");
        services.AddDbContext<NotificationDbContext>(options => options.UseSqlServer(xxx, builder => builder.UseRowNumberForPaging()));


        //var yy = configuration.GetConnectionString("BiEventDbConnection");
        //services.AddDbContext<BiEventDbContext>(options => options.UseSqlServer(yy, builder => builder.UseRowNumberForPaging()));

        return services;
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

    public static void AddUriPaginationService(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddSingleton<IUriService>(o =>
        {
            var accessor = o.GetRequiredService<IHttpContextAccessor>();
            var request = accessor.HttpContext.Request;
            var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
            return new UriService(uri);
        });
    }
}
