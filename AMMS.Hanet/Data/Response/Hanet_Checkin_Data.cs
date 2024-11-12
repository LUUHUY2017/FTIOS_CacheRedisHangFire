namespace AMMS.Hanet.Data.Response
{
    /// <summary>
    /// Khi có event xảy ra trên thiết bị, như có người nhân viên, khách hàng checkin, hoặc có người lạ xuất hiện, HANET sẽ chủ động đẩy data về webhook của đối tác đã đăng kí trước.
    /// </summary>
    public class Hanet_Checkin_Data
    {
        /// <summary>
        /// type sẽ là"update" để xác định thao tác thêm mới/cập nhật
        /// </summary>
        public string? action_type { get; set; }
        /// <summary>
        /// là id định danh của một FaceID.
        /// </summary>
        public string? aliasID { get; set; }
        /// <summary>
        /// type là "log" để xác định loại dữ liệu là dữ liệu chấm công
        /// </summary>
        public string? data_type { get; set; }
        /// <summary>
        /// Date format: YYYY-MM-DD HH:mm:ss
        /// </summary>
        public DateTime? date { get; set; }
        /// <summary>
        /// link checkin của FaceID.
        /// </summary>
        public string? detected_image_url { get; set; }
        /// <summary>
        /// là id của device
        /// </summary>
        public string? deviceID { get; set; }
        /// <summary>
        /// là tên của device
        /// </summary>
        public string? deviceName { get; set; }
        /// <summary>
        ///  MD5 của "client_secret" + "id", dùng để verify record này là được gửi từ HANET
        /// </summary>
        public string? hash { get; set; }
        /// <summary>
        /// unique record ID
        /// </summary>
        public string? id { get; set; }
        /// <summary>
        /// là token định danh của đối tác gửi cho HANET khi authen qua Oauth
        /// </summary>
        public string? keycode { get; set; }
        /// <summary>
        ///  id định danh của một FaceID
        /// </summary>
        public string? personID { get; set; }
        /// <summary>
        /// tên của FaceID
        /// </summary>
        public string? personName { get; set; }
        /// <summary>
        /// chức danh của FaceID
        /// </summary>
        public string? personTitle { get; set; }
        /// <summary>
        ///  có các giá trị (0,1,2,3,4,5,6) trong đó giá trị (0 là Nhân viên) hoặc (1 là Khách hàng) còn (2,3,4,5 là người lạ), (6 là ảnh chụp hình từ camera).
        /// </summary>
        public int? personType { get; set; }
        /// <summary>
        /// là ID của địa điểm mà camera đang dùng
        /// </summary>
        public string? placeID { get; set; }
        /// <summary>
        ///  là tên địa điểm.
        /// </summary>
        public string? placeName { get; set; }
        /// <summary>
        ///  thông tin có đeo khẩu trang hay ko (-1: không có bật tính năng kiểm tra khẩu trang, 0: không đeo khẩu trang, 2: có đeo khẩu trang)
        /// </summary>
        public string? mask { get; set; }
        /// <summary>
        ///  Timestamp tại thời điểm camera checkin.
        /// </summary>
        public double? time { get; set; }
    }

    public class Hanet_Checkin_Data_History
    {
        /// <summary>
        ///  id định danh của một FaceID
        /// </summary>
        public string? personID { get; set; }
        /// <summary>
        /// tên của FaceID
        /// </summary>
        public string? personName { get; set; }

        /// <summary>
        /// là id định danh của một FaceID.
        /// </summary>
        public string? aliasID { get; set; }
        /// <summary>
        /// type là "log" để xác định loại dữ liệu là dữ liệu chấm công
        /// </summary>
        public int? type { get; set; }
        /// <summary>
        /// Date format: YYYY-MM-DD 
        /// </summary>
        public DateTime? date { get; set; }
        /// <summary>
        /// link checkin của FaceID.
        /// </summary>
        public string? avatar { get; set; }
        /// <summary>
        /// là id của device
        /// </summary>
        public string? deviceID { get; set; }
        /// <summary>
        /// là tên của device
        /// </summary>
        public string? deviceName { get; set; }
        /// <summary>
        ///  Data
        /// </summary>
        public string? data { get; set; }
        /// <summary>
        /// chức danh của FaceID
        /// </summary>
        public string? title { get; set; }
        /// <summary>
        /// là ID của địa điểm mà camera đang dùng
        /// </summary>
        public int? placeID { get; set; }
        /// <summary>
        ///  là tên địa điểm.
        /// </summary>
        public string? place { get; set; }
        /// <summary>
        ///  Timestamp tại thời điểm camera checkin.
        /// </summary>
        public double? checkinTime { get; set; }
    }

}
