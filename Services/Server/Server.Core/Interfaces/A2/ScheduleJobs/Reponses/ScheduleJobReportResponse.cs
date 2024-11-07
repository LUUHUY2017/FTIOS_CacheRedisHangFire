using Server.Core.Entities.A2;

namespace Server.Core.Interfaces.A2.ScheduleJobs.Reponses;
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
