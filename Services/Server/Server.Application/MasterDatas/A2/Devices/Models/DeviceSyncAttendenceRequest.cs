namespace Server.Application.MasterDatas.A2.Devices.Models
{
    public class DeviceSyncAttendenceRequest
    {
        public string? Id { get; set; }
        public string? OrganizationId { get; set; }
        public string? SerialNumber { get; set; }
        public string? DeviceName { get; set; }
        public string? DeviceType { get; set; }
        public string? DeviceModel { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }





    }
}
