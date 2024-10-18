using Microsoft.AspNetCore.SignalR.Client;
using Shared.Core.SignalRs;

namespace Shared.Core.SignalRs;
public class SignalRAdapter : ISignalRAdapter
{
    public HubConnection? Connection { get; set; }

    public SignalRAdapter()
    {
        //Start("https://localhost:5001");
    }
    public void Start(string url)
    {
        if (Connection == null)
        {
            if (!url.EndsWith("/"))
                url += "/";
            try
            {
                Connection = new HubConnectionBuilder()
               .WithUrl(url + "ammshub")
               .WithAutomaticReconnect()
               .Build()
               ;

                Connection.StartAsync().Wait();
                //Logger.ShowLog("Đã kết nối SignalR");
            }
            catch (Exception ex)
            {
                //Logger.ShowLog("Lỗi kết nối SignalR: " + ex.Message);
                //Logger.ShowLog(ex);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fun">Tên hàm/chức năng</param>
    /// <param name="clientId">ID của socket client</param>
    /// <param name="data">Dữ liệu dưới dạng string</param>
    /// <param name="message"></param>
    public async void Notification(string fun, string clientId, string data, string message)
    {
        if (Connection != null)
        {
            switch (Connection.State)
            {
                case HubConnectionState.Connected:
                    await Connection.InvokeAsync("AMMS_SignalR_Response", fun, clientId, data, true, message);
                    break;
                case HubConnectionState.Connecting:
                    // code block
                    break;
                case HubConnectionState.Reconnecting:
                    // code block
                    break;
                case HubConnectionState.Disconnected:
                    // code block
                    break;
                default:
                    // code block
                    break;
            }
        }
    }
}
