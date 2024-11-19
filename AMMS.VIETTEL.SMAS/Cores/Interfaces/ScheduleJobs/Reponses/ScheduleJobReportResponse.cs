using AMMS.VIETTEL.SMAS.Cores.Entities.A2;

namespace AMMS.VIETTEL.SMAS.Cores.Interfaces.ScheduleJobs.Reponses;
public class ScheduleJobReportResponse : ScheduleJob
{
    /// <summary>
    /// Loại đồng bộ
    /// </summary>
    public string? ScheduleTypeName { get; set; }

    /// <summary>
    /// Tuần suất gửi
    /// </summary>
    public string? ScheduleSequentialName { get; set; }
    public string? OrganizationName { get; set; }

}
