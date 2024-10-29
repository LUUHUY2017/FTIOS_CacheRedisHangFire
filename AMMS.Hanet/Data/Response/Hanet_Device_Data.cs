namespace AMMS.Hanet.Data.Response
{
    /// <summary>
    /// Khi user có thao tác liên quan đến việc thêm mới, cập nhật thông tin thiết bị, HANET sẽ chủ động đẩy data thông báo cho đối tác
    /// </summary>
    public class Hanet_Device_Data
    {
        /// <summary>
        /// type sẽ là"update" để xác định thao tác thêm mới/cập nhật
        /// </summary>
        public string? action_type { get; set; }
        public string? data_type { get; set; }
        public DateTime? date { get; set; }
        public string? deviceID { get; set; }
        public string? deviceName { get; set; }
        public string? hash { get; set; }
        public string? id { get; set; }
        public string? keycode { get; set; }
        public string? placeID { get; set; }
        public string? placeName { get; set; }
        public double? time { get; set; }

    }
}
