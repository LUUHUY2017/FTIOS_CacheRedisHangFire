using Microsoft.AspNetCore.SignalR.Client;

namespace Shared.Core.SignalRs;
/// <summary>
/// Kết nối đến SignalRserver
/// </summary>
public interface ISignalRAdapter
{
    HubConnection Connection { get; set; }
    void Start(string url);
}
