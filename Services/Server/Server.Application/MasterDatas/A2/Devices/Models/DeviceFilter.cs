namespace Server.Application.MasterDatas.A2.Devices.Models
{
    public class DeviceFilter
    {
        public string? ColumnTable { get; set; }
        public string? Key { get; set; } = "";
        public string? KeySearch { get; set; } = "";
        public int? OrganizationId { get; set; } = -1;
        public int? SiteId { get; set; } = -1;
        public int? LocationId { get; set; } = -1;
    }
}
