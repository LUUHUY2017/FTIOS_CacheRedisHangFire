namespace AMMS.VIETTEL.SMAS.APIControllers.ScheduleSendMails.V1.Requests;

public class ScheduleSendEmailDetailRequest
{
    public int? Id { get; set; }

    public int? OrganizationId { get; set; }

    /// <summary>
    /// Trạng thái
    /// </summary>
    public bool? Actived { get; set; }

    public DateTime? CreateTime { get; set; } = DateTime.Now;
    public DateTime? ModifyTime { get; set; }

    /// <summary>
    /// Tên lịch báo cáo
    /// </summary>
    public int? ScheduleId { get; set; }

    /// <summary>
    /// Email nhận báo cáo
    /// </summary>
    public string? ScheduleEmail { get; set; }
    /// <summary>
    /// Tên người nhận báo cáo
    /// </summary>
    public string? ScheduleUserName { get; set; }

}

