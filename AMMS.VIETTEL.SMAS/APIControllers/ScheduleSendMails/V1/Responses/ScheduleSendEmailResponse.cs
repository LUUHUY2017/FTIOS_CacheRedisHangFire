using AMMS.VIETTEL.SMAS.Cores.Entities.A2;

namespace AMMS.VIETTEL.SMAS.APIControllers.ScheduleSendMails.V1.Responses;
public class ScheduleSendEmailResponse : ScheduleSendMail
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
