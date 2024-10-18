using EventBus.Messages;
using MassTransit;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Shared.Core.Commons;
using Shared.Core.Emails.V1.Adapters;
using Shared.Core.Emails.V1.Commons;
using Shared.Core.Loggers;

namespace Shared.Core.Emails.V1;

public class EmailSenderServiceV1
{
    private readonly IEventBusAdapter _eventBusAdapter;
    private readonly IConfiguration _configuration;

    private readonly SMTP_Email_Adpater _sMTP_EmailAdpater;

    public EmailSenderServiceV1(IBus bus
     , IConfiguration configuration
     , IOptions<EventBusSettings> eventBusSettings
     , IEventBusAdapter eventBusAdapter
     , SMTP_Email_Adpater sMTP_EmailAdpater
     )
    {
        _configuration = configuration;
        _eventBusAdapter = eventBusAdapter;
        _sMTP_EmailAdpater = sMTP_EmailAdpater;
    }

    /// <summary>
    /// Gửi email qua RabbitMQ
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<Result<string>> SendByEventBusAsync(SendEmailMessageRequest1 request)
    {
        try
        {
            var aa = await _eventBusAdapter.GetSendEndpointAsync(_configuration.GetValue<string>("DataArea") + EmailConst.EventBusChanelSendEmail);
            await aa.Send(request);
            return new Result<string>($"Gửi thành công", true);
        }
        catch (Exception e)
        {
            Logger.Error(e);
            return new Result<string>($"Gửi email lỗi: {e.Message}", false);
        }
        
    }
    /// <summary>
    /// Gửi email trực tiếp
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<Result<string>> SendAsync(SendEmailMessageRequest1 request)
    {
        try
        { 
            // Gửi email
            var retVal = await _sMTP_EmailAdpater.SendEmailAsync(request);
            return retVal;
        }
        catch (Exception e)
        {
            Logger.Error(e);
            return new Result<string>($"Gửi email lỗi: {e.Message}", false);
        }
    }
}
