namespace Server.Application.MasterDatas.A2.ScheduleJobs.V1.Models;

public class ScheduleJobRequest
{
    public string? Id { get; set; }

    public string? OrganizationId { get; set; }

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

}
