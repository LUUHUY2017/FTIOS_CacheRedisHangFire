using Shared.Core.Commons;

namespace Server.Core.Interfaces.A2.ScheduleSendEmails.Requests;

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
