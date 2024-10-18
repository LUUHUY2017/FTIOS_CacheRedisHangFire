namespace Server.Application.MasterDatas.A2.Devices.Models
{
    public class TotalDevicesSite
    {
        public string? OrganizationId { get; set; }
        public string? OrganizationName { get; set; }
        public int? SiteId { get; set; }
        public string? SiteName { get; set; } = "";
        public int? Total { get; set; } = 0;
        public int? TotalOnline { get; set; } = 0;
        public int? TotalOffline { get; set; } = 0;
        public int? TotalLocation { get; set; } = 0;
    }
}
