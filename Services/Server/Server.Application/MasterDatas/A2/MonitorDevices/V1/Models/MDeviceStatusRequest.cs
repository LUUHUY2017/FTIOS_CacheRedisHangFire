namespace Server.Application.MasterDatas.A2.MonitorDevices.V1.Models;

public class MDeviceStatusRequest
{
    public string? SerialNumber { get; set; }
    public bool? ConnectionStatus { get; set; }
    public DateTime? ConnectUpdateTime { get; set; }
}
