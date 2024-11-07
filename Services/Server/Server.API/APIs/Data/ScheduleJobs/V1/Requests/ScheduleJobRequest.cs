namespace Server.API.APIs.Data.ScheduleSendMails.V1.Requests;

public class ScheduleJobRequest
{
    public int? Id { get; set; }

    public int? OrganizationId { get; set; }

    /// <summary>
    /// Trạng thái
    /// </summary>
    public bool? Actived { get; set; }

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


    public string? ScheduleTimeSend { get; set; }

    public string? ScheduleTimeStart { get; set; }

    public string? ScheduleTimeEnd { get; set; }

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
    /// Ngày lấy dữ liệu: hiện tại và trước đó Today, Yesterday
    /// </summary>
    public string? ScheduleDataCollect { get; set; }

}
