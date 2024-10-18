using EventBus.Messages;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Shared.Core.Emails.V1.Adapters;
using Shared.Core.Emails.V1.Commons;
using Shared.Core.Loggers;
namespace Shared.Core.Emails.V1;
public class EmailRabbitMQConsummerV1 : IConsumer<SendEmailMessageRequest1>
{
    private readonly SMTP_Email_Adpater _sMTP_EmailAdpater;
    private readonly IEventBusAdapter _eventBusAdapter;
    private readonly IConfiguration _configuration;
    private readonly EventBusSettings _eventBusSettings;
    public EmailRabbitMQConsummerV1(
        SMTP_Email_Adpater sMTP_EmailAdpater
        , IConfiguration configuration
        , IOptions<EventBusSettings> eventBusSettings
        , IEventBusAdapter eventBusAdapter
        )
    {
        _sMTP_EmailAdpater = sMTP_EmailAdpater;

        _configuration = configuration;
        _eventBusAdapter = eventBusAdapter;
        _eventBusSettings = eventBusSettings.Value;
    }

    public async Task Consume(ConsumeContext<SendEmailMessageRequest1> context)
    {
        try
        {
            // Gửi email
            var retVal = await _sMTP_EmailAdpater.SendEmailAsync(context.Message);
            // Trả kết quả qua eventsbus
            var aa = await _eventBusAdapter.GetSendEndpointAsync(_configuration.GetValue<string>("DataArea") + EmailConst.EventBusChanelSendEmailResponse);
            await aa.Send(new SendEmailMessageResponse1() { Id = context.Message.Id, SendStatus = retVal.Succeeded, Message = retVal.Message });
            await Task.Delay(500);
        }
        catch (Exception e)
        {
            Logger.Error(e);
        }
    }

}
