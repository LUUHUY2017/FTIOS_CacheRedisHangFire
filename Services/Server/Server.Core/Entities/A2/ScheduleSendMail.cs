using Shared.Core.Commons;
using Shared.Core.Entities;

namespace Server.Core.Entities.A2;

public class ScheduleSendMail : EntityBase
{

    public string? OrganizationId { get; set; }

    /// <summary>
    /// Tên lịch báo cáo
    /// </summary>
    public string? ScheduleName { get; set; }

    /// <summary>
    /// Loại file xuất báo cáo
    /// </summary>
    public string? ScheduleExportType { get; set; }

    /// <summary>
    /// Loại báo cáo
    /// </summary>
    public string? ScheduleReportType { get; set; }

    /// <summary>
    /// Tuần suất gửi
    /// </summary>
    public string? ScheduleSequentialSending { get; set; }

    /// <summary>
    /// Tham số
    /// </summary>
    public string? ScheduleParam { get; set; }

    /// <summary>
    /// Thời gian gửi báo cáo
    /// </summary>
    public TimeSpan? ScheduleTimeSend { get; set; }

    /// <summary>
    /// Thời gian bắt đầu lấy dữ liệu
    /// </summary>
    public TimeSpan? ScheduleTimeStart { get; set; }

    /// <summary>
    /// Thời gian kết thúc lấy dữ liệu
    /// </summary>
    public TimeSpan? ScheduleTimeEnd { get; set; }

    /// <summary>
    /// Tiêu đề email
    /// </summary>
    public string? ScheduleTitleEmail { get; set; }
    /// <summary>
    /// Nội dung 
    /// </summary>
    public string? ScheduleContentEmail { get; set; }


    public string? ScheduleNote { get; set; }
    public string? OrganizationName { get; set; }

    /// <summary>
    /// Ngày lấy dữ liệu: hiện tại và trước đó Current, Before
    /// </summary>
    public string? ScheduleDataCollect { get; set; }
    public bool? ReportSent { get; set; }

}

public class ListScheduleEmailCategory
{
    public static List<ObjectString> ReportType = new List<ObjectString>()
    {
                 new ObjectString() { Id = "BAOCAOCHITIETNGAY", Name = "Báo cáo chi tiết ngày" },
                 new ObjectString() { Id = "BAOCAOCHITIETTHANG", Name = "Báo cáo chi tiết tháng" },
    };
    public static List<ObjectString> ReportTypeDevice = new List<ObjectString>()
    {
                 new ObjectString() { Id = "CANHBAOTHIETBIMATKETNOI", Name = "Cảnh báo thiết bị mất kết nối" },
    };
    //compare report
    public static List<ObjectString> ReportTypeInOut = new List<ObjectString>()
    {
               new ObjectString() { Id = "BAOCAOSOSANH", Name = "Báo cáo so sánh theo ngày" },
    };
    public static List<ObjectString> SequentialSendingInOut = new List<ObjectString>()
    {
               new ObjectString() { Id = "Daily", Name = "Hàng ngày" },
    };
    //
    public static List<ObjectString> SequentialSending = new List<ObjectString>()
      {
               new ObjectString() { Id = "Daily", Name = "Hàng ngày" },
               new ObjectString() { Id = "Monthly", Name = "Hàng tháng" },
      };
    public static List<ObjectString> SequentialSendingDevice = new List<ObjectString>()
    {
               new ObjectString() { Id = "Hourly", Name = "Hàng giờ" },
               new ObjectString() { Id = "TwoHourly", Name = "Mỗi 2 giờ" },
    };

    public static List<ObjectString> ExportType = new List<ObjectString>()
    {
               new ObjectString() { Id = "Excel", Name = "Excel" },
    };

    public static List<ObjectString> DataCollectType = new List<ObjectString>()
    {
             new ObjectString() { Id = "Current", Name = "Hiện tại" },
             new ObjectString() { Id = "Before", Name = "Trước đó" },
    };
    /// <summary>
    /// Loại lập lịch
    /// </summary>
    public static List<ObjectString> ScheduleNote = new List<ObjectString>()
    {
         new ObjectString() { Id = "BAOCAOTUDONG", Name = "Báo cáo tự động" },
         new ObjectString() { Id = "CANHBAOTHIETBIMATKETNOI", Name = "Cảnh báo thiết bị mất kết nối" },
         new ObjectString() { Id = "BAOCAOSOSANH", Name = "Báo cáo so sánh" },
    };

}



