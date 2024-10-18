namespace Server.Application.MasterDatas.A2.Devices.Models
{
    public class DeviceSiteListFilter
    {
        public int OrganizationId { get; set; }
        public List<int> SiteIds { get; set; }
        public string Key { get; set; } = "";
    }
}
