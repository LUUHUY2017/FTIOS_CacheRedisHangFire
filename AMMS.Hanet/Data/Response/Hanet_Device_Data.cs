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
        public string? data { get; set; }
    }
}
