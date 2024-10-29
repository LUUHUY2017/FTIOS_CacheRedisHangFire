namespace AMMS.DeviceData.RabbitMq
{
    public class RB_ServerResponse
    {
        public string Id { get; set; }
        public string? RequestId { get; set; }
        public string? ReponseType { get; set; }
        public string? Action { get; set; }
        public string? Content { get; set; }
        public bool? IsSuccessed { get; set; }
        public string? Message { get; set; }
        public DateTime? DateTimeResponse { get; set; }

    }
    public class RB_ServerResponseType
    {
        public static string UserInfo = "UserInfo";
        public static string UserImage = "UserImage";
    }
    public class RB_ServerResponseMessage
    {
        public static string Complete = "Thành công";
        public static string InComplete = "Không thành công";
    }
}
