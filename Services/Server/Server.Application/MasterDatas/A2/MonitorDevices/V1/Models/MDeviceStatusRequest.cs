namespace Server.Application.MasterDatas.A2.MonitorDevices.V1.Models;

public class MDeviceStatusRequest
{
    /// <summary>
    /// Serrial number của thiết bị
    /// </summary>
    public string? serialNumber { get; set; } = "";
    /// <summary>
    /// Trạng thái thiết bị
    /// </summary>
    public bool? connectionStatus { get; set; } = false;
    /// <summary>
    /// Thời gian kiểm tra
    /// </summary>
    public DateTime? connectUpdateTime { get; set; } = DateTime.Now;
    /// <summary>
    /// Thời gian kết nối
    /// </summary>
    public DateTime? time_online { get; set; }
    /// <summary>
    /// Thời gian mất kết nôi
    /// </summary>
    public DateTime? time_offline { get; set; }
}
