namespace AMMS.Hanet.Data.Response
{
    /// <summary>
    /// Khi user tạo mới hoặc cập nhật thông tin Địa Điểm, HANET sẽ chủ động đẩy data về webhook của đối tác đã đăng kí trước.
    /// </summary>
    public class Hanet_Place_Data
    {
        public string? action_type { get; set; }
        public string? data_type { get; set; }
        public DateTime? date { get; set; }
        public string? hash { get; set; }
        public string? id { get; set; }
        public string? keycode { get; set; }
        public string? placeID { get; set; }
        public string? placeName { get; set; }
        public double? time { get; set; }

    }
}
