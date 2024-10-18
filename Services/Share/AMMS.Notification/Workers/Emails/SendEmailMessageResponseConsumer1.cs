using EventBus.Messages;
using MassTransit;
using Shared.Core.Emails.V1.Commons;
using Shared.Core.Loggers;
namespace AMMS.Notification.Workers.Emails;

public class SendEmailMessageResponseConsumer1 : IConsumer<SendEmailMessageResponse1>
{
    private readonly IEventBusAdapter _eventBusAdapter;
    private readonly SendEmailMessageService1 _sendEmailMessageService1;


    public SendEmailMessageResponseConsumer1(
              IEventBusAdapter eventBusAdapter
        , SendEmailMessageService1 sendEmailMessageService1
        )
    {
        _eventBusAdapter = eventBusAdapter;
        _sendEmailMessageService1 = sendEmailMessageService1;
    }

    public async Task Consume(ConsumeContext<SendEmailMessageResponse1> context)
    {
        try
        {
            Console.WriteLine($"Nhận kết quả sau khi gửi email trả lại qua eventbus {context.Message.Id}: {context.Message.Message}: {context.Message.SendStatus}");
            //Lưu kết quả gửi email vào lịch sử gửi email CSDL
            await _sendEmailMessageService1.UpdateSendStatus(context.Message);
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
    }
}
