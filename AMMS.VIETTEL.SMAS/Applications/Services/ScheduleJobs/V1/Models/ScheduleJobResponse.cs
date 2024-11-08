using AMMS.VIETTEL.SMAS.Cores.Entities.A2;

namespace AMMS.VIETTEL.SMAS.Applications.Services.ScheduleJobs.V1.Models;
public class ScheduleJobResponse : ScheduleJob
{
    /// <summary>
    /// Loại đồng bộ
    /// </summary>
    public string? ScheduleJobTypeName { get; set; }

    /// <summary>
    /// Tuần suất gửi
    /// </summary>
    public string? ScheduleSequentialName { get; set; }
    public string? OrganizationName { get; set; }

}
