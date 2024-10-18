using EventBus.Messages;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;

namespace Shared.Core.SignalRs;

public class SignalRService : ISignalRService
{
    private readonly ISignalRClientService _adapter;
    private readonly IEventBusAdapter _eventBusAdapter;

    public SignalRService(ISignalRClientService adapter
        , IEventBusAdapter eventBusAdapter)
    {
        _adapter = adapter;
        _eventBusAdapter = eventBusAdapter;
    }

    public async Task<bool> PushNotfiti(string userid, string title, string body)
    {
        //Đẩy thông báo lên SignalR Server
        if (_adapter.Connection != null && _adapter.Connection.State == HubConnectionState.Connected)
        {
            await _adapter.Connection.InvokeAsync("AMMS_SignalR_Notification", "Device_Status_Online", "all", JsonConvert.SerializeObject(new { userid, title, body }), "");
        }
        return false;
    }

    public async Task<bool> PushNotfitiQueue(string userid, string title, string body)
    {
        //Đẩy vào Queue
        var endPoint = await _eventBusAdapter.GetSendEndpointAsync("");
        await endPoint.Send(body);

        return true;
    }

    public async Task<bool> Pub(string method, string dataJson)
    {
        //Đẩy thông báo lên SignalR Server
        if (_adapter.Connection != null && _adapter.Connection.State == HubConnectionState.Connected)
        {
            await _adapter.Connection.InvokeAsync(method,   dataJson);
        }
        return false;
    }
}