using Shared.Core.Commons;

namespace AMMS.VIETTEL.SMAS.Cores.Interfaces.ScheduleJobs.Requests;

public class ScheduleJobFilterRequest : BaseFilterRequest
{
    public string? OrganizationId { get; set; } = "0";
    /// <summary>
    /// Loại lập lịch
    /// </summary>
    public string? ScheduleNote { get; set; }
}