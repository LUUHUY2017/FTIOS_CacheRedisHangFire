using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Core.Emails.V1.Extensions;

public static class DependencyInjection
{
    public static void AddEventBusSendEmailV1(this IServiceCollection services, IConfiguration configuration)
    {

        ////////////////
        ///Nhận qua event bus để gửi
        ////////////////
        //services.AddScoped<EmailRabbitMQConsummerV1>();

        //services.AddMassTransit(config =>
        //{
        //    //config.AddConsumer<EmailSendingConsumer>();

        //    //Đăng ký xử lý bản tin data XML của Brickstream
        //    config.AddConsumer<EmailRabbitMQConsummerV1>();


        //    config.UsingRabbitMq((ct, cfg) =>
        //    {
        //        //cfg.Host(configuration["EventBusSettings:HostAddress"], h =>
        //        //{
        //        //    h.Username("guest");
        //        //    h.Password("guest");
        //        //});

        //        cfg.Host(configuration["EventBusSettings:HostAddress"]);


        //        //provide the queue name with consumer settings
        //        cfg.ReceiveEndpoint($"{configuration["DataArea"]}_{EmailConst.EventBusChanelSendEmail}", c =>
        //        {
        //            c.ConfigureConsumer<EmailRabbitMQConsummerV1>(ct);
        //        });


        //        cfg.ConfigureEndpoints(ct);
        //    });
        //});
    }

}