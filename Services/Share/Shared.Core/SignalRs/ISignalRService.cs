namespace Shared.Core.SignalRs;

public interface ISignalRService
{
    //Đẩy trực tiếp lên signalR
    Task<bool> PushNotfiti(string userid, string title, string body);
    //Đẩy lên signalR qua Queue
    Task<bool> PushNotfitiQueue(string userid, string title, string body);
    Task<bool> Pub(string method, string dataJson);
}
