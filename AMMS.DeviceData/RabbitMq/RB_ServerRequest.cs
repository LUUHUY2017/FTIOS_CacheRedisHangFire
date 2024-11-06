namespace AMMS.DeviceData.RabbitMq
{
    public class RB_ServerRequest
    {
        public string Id { get; set; }
        /// <summary>
        /// Loại sự kiện
        /// </summary>
        public string Action { get; set; }
        /// <summary>
        /// Đối tượng request
        /// </summary>
        public string RequestType { get; set; }
        /// <summary>
        /// Id thiết bị
        /// </summary>
        public string DeviceId { get; set; }
        /// <summary>
        /// SerialNumber thiết bị
        /// </summary>
        public string SerialNumber { get; set; }
        /// <summary>
        /// Loại thiết bị
        /// </summary>
        public string DeviceModel { get; set; }
        /// <summary>
        ///  Json obj requet
        /// </summary>
        public string? RequestParam { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? SchoolId { get; set; }

    }
    public class ServerRequestAction
    {
        public static string ActionAdd = "Add";
        public static string ActionDelete = "Delete";
        public static string ActionGetData = "GetData";
        public static string ActionGetDeviceInfo = "GetDeviceInfo";
    }
    public class ServerRequestType
    {
        public static string UserInfo = "UserInfo";
        public static string UserFace = "UserFace";
        public static string UserFinger = "UserFinger";
        public static string Device = "Device";

    }
}
