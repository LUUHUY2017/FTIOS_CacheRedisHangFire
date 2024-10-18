using EventBus.Messages;
using MassTransit;
using Shared.Core.Emails.V1.Commons;
using Shared.Core.Loggers;
namespace AMMS.Notification.EventBusConsumer;
public class NotificationConsumer : IConsumer<SendEmailMessageResponse1>
{
    private readonly IEventBusAdapter _eventBusAdapter;


    public NotificationConsumer(
              IEventBusAdapter eventBusAdapter
        )
    {
        _eventBusAdapter = eventBusAdapter;
    }

    public async Task Consume(ConsumeContext<SendEmailMessageResponse1> context)
    {
        try
        {
            //Lưu vào CSDL

            //Đẩy vào kênh Email

            //Đẩy vào kênh SignalR

            //Đẩy vào kênh firebase

            //Đẩy vào kênh SMS

            //Đẩy vào kênh zalo

        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
    }

}

