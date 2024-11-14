namespace Server.Core.Interfaces.A2.DeviceNotifications
{
    public class DeviceNotificationModel
    {
        public string? OrganizationId { get; set; }
        public string? ColumnTable { get; set; }
        public string? Actived { get; set; }
        public string? Key { get; set; }
        public string? Note { get; set; } = "";
        public string? Export { get; set; } = "0";
    }

    public class DeviceNotificationLogModel
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Sent { get; set; } = "";

    }
}
