using AMMS.Notification.EventBusConsumer;
using EventBus.Messages;
using EventBus.Messages.Common;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Core.SignalRs;
namespace AMMS.Notification.Extensions;

public static class DependencyInjection
{
    public static void AddNotification(this IServiceCollection service, IConfiguration configuration)
    {
        //service.AddScoped<ICacheService, CacheService>();


        AddEventBusService(service, configuration);
        AddSignalRService(service, configuration);
    }
    static void AddSignalRService(this IServiceCollection service, IConfiguration configuration)
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
    static void AddEventBusService(this IServiceCollection services, IConfiguration configuration)
    {
        var eventBusSettings = configuration.GetSection("EventBusSettings");  // đọc config
        services.Configure<EventBusSettings>(eventBusSettings);
        services.AddScoped<IEventBusAdapter, EventBusAdapter>();

        //services.AddScoped<EmailSendingConsumer>();


        services.AddScoped<NotificationConsumer>();



        services.AddMassTransit(config =>
        {
            //config.AddConsumer<EmailSendingConsumer>();

            config.AddConsumer<NotificationConsumer>();


            config.UsingRabbitMq((ct, cfg) =>
            {
                //cfg.Host(configuration["EventBusSettings:HostAddress"], h =>
                //{
                //    h.Username("guest");
                //    h.Password("guest");
                //});

                cfg.Host(configuration["EventBusSettings:HostAddress"]);


                //provide the queue name with consumer settings

                //Email Sending
                //cfg.ReceiveEndpoint(EventBusConstants.EmailSendingQueue, c =>
                //{
                //    c.ConfigureConsumer<EmailSendingConsumer>(ct);
                //});

                cfg.ReceiveEndpoint($"{configuration["DataArea"]}{EventBusConstants.Notification_V1}", c =>
                {
                    c.ConfigureConsumer<NotificationConsumer>(ct);
                });


                cfg.ConfigureEndpoints(ct);
            });
        });

        services.AddMassTransitHostedService();
    }


}
