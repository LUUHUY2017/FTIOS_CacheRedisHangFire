namespace EventBus.Messages.Common;
public static class EventBusConstants
{   
    public static string EmailSendingQueue { get; private set; } = $"_amms-sending_email-queue";
    public static string BrickstreamXMLData { get; private set; } = $"_brickstream-data-xml";
    public static string BrickstreamData { get; private set; } = $"_brickstream-data";
    public static string DeviceOnlineOffline_Queue { get; private set; } = $"_device-on_off-queue";
    public static string Notification_V1 { get; private set; } = $"_notification_v1"; 
} 

