using Shared.Core.Commons;

namespace AMMS.VIETTEL.SMAS.Cores.Interfaces.ScheduleSendEmails;

public class ScheduleSendEmailFilterRequest : BaseFilterRequest
{
    public string? OrganizationId { get; set; } = "0";
    /// <summary>
    /// Loại lập lịch
    /// </summary>
    public string? ScheduleNote { get; set; }

}
public class ScheduleSendEmailLogModel
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Sent { get; set; } = "";
    public string? OrganizationId { get; set; } = "0";

}
