using EventBus.Messages;
using MassTransit;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Shared.Core.Loggers;

namespace Server.Application.Devices.EventBusConsumer.V1;
public class DeviceConsumer : IConsumer<Terminal>
{

    private readonly IEventBusAdapter _eventBusAdapter;

    private readonly Shared.Core.SignalRs.ISignalRClientService _signalRClientService;
    public DeviceConsumer(
        IEventBusAdapter eventBusAdapter
      , Shared.Core.SignalRs.ISignalRClientService signalRClientService
        )
    {
        _eventBusAdapter = eventBusAdapter;
        _signalRClientService = signalRClientService;
    }

    public async Task Consume(ConsumeContext<Terminal> context)
    {
        //Noti thiết bị Online/Offline
        try
        {
            var xxx = JsonConvert.SerializeObject(context);
            if (_signalRClientService.Connection != null && _signalRClientService.Connection.State == HubConnectionState.Connected)
            {
                _signalRClientService.Connection.SendAsync("MonitorDevice", xxx);
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
        //Gửi email thông báo thiết bị online/offline
        try
        {
            if (context.Message.Status == "Online")
            {
                string message = $"Thiết bị {context.Message.SerialNumber}, {context.Message.IpAddress} mất kế nối lúc {context.Message.LastTimeOffline}";
                //_mailAdpater.SendEmailAsync(new MailRequest() { EmailBody = message, FromEmail = "noreply@acs.vn", EmailSubject = $"Thiết bị mất kết nối", ToEmail = "nguyencongquyet@gmail.com" });
                Logger.Information(message);
            }
            else
            {
                string message = $"Thiết bị {context.Message.SerialNumber}, {context.Message.IpAddress} kết nối lúc {context.Message.LastTimeOffline}";
                //_mailAdpater.SendEmailAsync(new MailRequest() { EmailBody = message, FromEmail = "noreply@acs.vn", EmailSubject = $"Thiết bị mất kết nối", ToEmail = "nguyencongquyet@gmail.com" });
                Logger.Information(message);
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }

    }
}

