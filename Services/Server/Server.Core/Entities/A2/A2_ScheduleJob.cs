using Shared.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Core.Entities.A2;

[Table("A2_ScheduleJob")]
public class A2_ScheduleJob : EntityBase
{
    [MaxLength(500)]
    public string? ScheduleJobName { get; set; }
    /// <summary>
    /// Thời gian gửi báo cáo
    /// </summary>
    public TimeSpan? ScheduleTimeSend { get; set; }
    /// <summary>
    /// Tuần suất gửi: 1 phút 1 lần, 1 ngày 1 lần, 
    /// </summary>
    /// 
    [MaxLength(250)]
    public string? ScheduleSequentialSending { get; set; }

    /// <summary>
    /// Loại đồng bộ: Học sinh, Điểm danh
    /// </summary>
    [MaxLength(250)]
    public string? Scheduleype { get; set; }
    [MaxLength(500)]
    public string? ScheduleNote { get; set; }
}
