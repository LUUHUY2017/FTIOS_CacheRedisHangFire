namespace Server.API.APIs.Data.ScheduleJobs.V1.Requests;

public class ScheduleJobRequest
{
    public int? Id { get; set; }

    public int? OrganizationId { get; set; }

    /// <summary>
    /// Trạng thái
    /// </summary>
    public bool? Actived { get; set; }

    /// <summary>
    /// Tên lịch báo cáo
    /// </summary>
    public string? ScheduleJobName { get; set; }


    /// <summary>
    /// Loại báo cáo
    /// </summary>
    public string? ScheduleType { get; set; }

    /// <summary>
    /// Tuần suất gửi
    /// </summary>
    public string? ScheduleSequential { get; set; }

  
    public string? ScheduleTime { get; set; }

    public string? ScheduleNote { get; set; }
    public string? OrganizationName { get; set; }

}
