using Microsoft.AspNetCore.SignalR;

namespace Server.API.SignalRs;

public class AmmsHub : Hub
{
    public AmmsHub()
    {
    }

    public async Task SystemLog(string type, string message)
    {
        await Clients.All.SendAsync("SystemLog", type, message);
    }

    public async Task ServerTime(string dateTime, long dateTimeTicks)
    {
        await Clients.All.SendAsync("ServerTime", dateTime, dateTimeTicks);
    }

    public async Task RefreshDevice(string data)
    {
        await Clients.All.SendAsync("RefreshDevice", data);
    }
    public async Task MonitorDevice(string type, string message)
    {
        await Clients.All.SendAsync("MonitorDevice", type, message);
    }

    public async Task RefreshSyncPage(string type, string message)
    {
        await Clients.All.SendAsync("RefreshSyncPage", type, message);
    }
}
