namespace Server.Application.MasterDatas.A2.Devices.Models
{
    public class TotalDevicesLocation
    {
        public int? LocationId { get; set; }
        public string? LocationName { get; set; } = "";
        public int? Total { get; set; } = 0;
        public int? TotalOnline { get; set; } = 0;
        public int? TotalOffline { get; set; } = 0;

    }
}
