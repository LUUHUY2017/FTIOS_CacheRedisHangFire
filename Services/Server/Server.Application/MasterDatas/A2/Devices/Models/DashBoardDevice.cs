namespace Server.Application.MasterDatas.A2.Devices.Models;

public class DashBoardDevice
{
    public List<DashBoardDeviceOrg> _dashBoardDevicesOrg { get; set; }
    public List<DashBoardDeviceSite> _dashBoardDevicesSite { get; set; }
}
public class DashBoardDeviceOrg
{
    public string? OrganizationName { get; set; }
    public string? OrganizationId { get; set; }
    public int? TotalSite { get; set; }
    public int? TotalDevice { get; set; }
    public int? TotalOnline { get; set; }
    public int? TotalOffline { get; set; }

}
public class DashBoardDeviceSite
{
    public string? SiteName { get; set; }
    public string? SiteId { get; set; }
    public int? TotalDevice { get; set; }

}
