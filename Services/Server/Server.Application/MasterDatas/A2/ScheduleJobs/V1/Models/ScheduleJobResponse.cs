using Server.Core.Entities.A2;

namespace Server.API.APIs.Data.ScheduleJobs.V1.Responses;
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
