namespace AMMS.DeviceData.RabbitMq
{
    public class RB_DataResponse
    {
        public string Id { get; set; }
        /// <summary>
        /// Loại trả về
        /// </summary>
        public string? ReponseType { get; set; }
        public string? Content { get; set; }
    }
    public class RB_DataResponseType
    {
        public static string AttendenceHistory = "AttendenceHistory";
        public static string AttendenceImage = "AttendenceImage";
        public static string UserInfo = "UserInfo";
        public static string AttendencePush = "AttendencePush";

    }
}
