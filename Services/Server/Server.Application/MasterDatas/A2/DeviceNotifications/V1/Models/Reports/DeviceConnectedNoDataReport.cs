namespace Server.Application.MasterDatas.A2.DeviceNotifications.V1.Models.Reports;

public class DeviceConnectedNoDataReport
{
    public int? OrganizationId { get; set; } = 0;
    public string? OrganizationName { get; set; }
    public int? SiteId { get; set; } = 0;
    public string? SiteName { get; set; }
    public int? LocationId { get; set; } = 0;
    public string? LocationName { get; set; }
    public string SerialNumber { get; set; } = null!;
    public string? DeviceName { get; set; }
    public string? WanIpAddress { get; set; }
    public string? IpAddress { get; set; }
    public int? HttpPort { get; set; }
    public int? HttpsPort { get; set; }
    public int TotalIn { get; set; } = 0;
    public int TotalOut { get; set; } = 0;
}
