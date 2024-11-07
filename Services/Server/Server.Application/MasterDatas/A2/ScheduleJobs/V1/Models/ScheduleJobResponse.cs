using Server.Core.Entities.A2;

namespace Server.Application.MasterDatas.A2.ScheduleJobs.V1.Models;
public class ScheduleJobResponse : ScheduleJob
{
    /// <summary>
    /// Loại đồng bộ
    /// </summary>
    public string? ScheduleJobTypeName { get; set; }

    /// <summary>
    /// Tuần suất gửi
    /// </summary>
    public string? ScheduleSequentialSendingName { get; set; }

}
