namespace Server.Application.MasterDatas.A2.Devices.Models
{
    public class DeviceRequest
    {
        public string? Id { get; set; }
        public bool? Actived { get; set; }

        public string? OrganizationId { get; set; }
        public string? LaneId { get; set; }

        public string? SerialNumber { get; set; }
        public string? DeviceName { get; set; }
        public string? DeviceID { get; set; }
        /// <summary>
        /// CAMERAIN & CAMERAOUT
        /// </summary>
        public string? DeviceType { get; set; }
        public string? Password { get; set; }

        public string? IpAddress { get; set; }
        public int? HTTPPort { get; set; }
        public int? PortConnect { get; set; }

        public string? DeviceDescription { get; set; }


    }
}
