namespace Server.Application.MasterDatas.A2.Devices.Models
{
    public class TotalDevicesOrg
    {
        public int? OrganizationId { get; set; }
        public string? OrganizationName { get; set; } = "";
        public int? Total { get; set; } = 0;
        public int? TotalOnline { get; set; } = 0;
        public int? TotalOffline { get; set; } = 0;
        public int? TotalSite { get; set; } = 0;
    }
}
