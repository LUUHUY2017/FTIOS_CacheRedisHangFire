using Server.Core.Entities.A2;

namespace Server.API.APIs.Data.ScheduleSendMails.V1.Responses;
public class ScheduleSendEmailResponse : A2_ScheduleSendMail
{
    /// <summary>
    /// Loại file xuất báo cáo
    /// </summary>
    public string? ScheduleExportTypeName { get; set; }

    /// <summary>
    /// Loại báo cáo
    /// </summary>
    public string? ScheduleReportTypeName { get; set; }

    /// <summary>
    /// Tuần suất gửi
    /// </summary>
    public string? ScheduleSequentialSendingName { get; set; }

    /// <summary>
    /// Ngày lấy dữ liệu
    /// </summary>
    public string? ScheduleDataCollectName { get; set; }

}
