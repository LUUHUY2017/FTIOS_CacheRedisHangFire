using Shared.Core.Commons;
using Shared.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Core.Entities.A2;

[Table("ScheduleJob")]
public class ScheduleJob : EntityBase
{
    [MaxLength(500)]
    public string? ScheduleJobName { get; set; }
    /// <summary>
    /// Thời gian chạy
    /// </summary>
    public TimeSpan? ScheduleTime { get; set; }
    /// <summary>
    /// Tuần suất gửi: 1 phút 1 lần, 1 ngày 1 lần, 
    /// </summary>
    /// 
    [MaxLength(250)]
    public string? ScheduleSequential { get; set; }

    /// <summary>
    /// Loại đồng bộ: Học sinh, Điểm danh
    /// </summary>
    [MaxLength(250)]
    public string? ScheduleType { get; set; }
    [MaxLength(500)]
    public string? ScheduleNote { get; set; }
}

public class ListScheduleCategory
{
    public static List<ObjectString> Sequentials = new List<ObjectString>()
      {
               new ObjectString() { Id = "Minutely", Name = "Mỗi phút" },
               new ObjectString() { Id = "Hourly", Name = "Hàng giờ" },
               new ObjectString() { Id = "Daily", Name = "Hàng ngày" },
               new ObjectString() { Id = "Weekly", Name = "Hàng tuần" },
               new ObjectString() { Id = "Monthly", Name = "Hàng tháng" },
               new ObjectString() { Id = "Yearly", Name = "Hàng năm" },
      };

    /// <summary>
    /// Loại lập lịch
    /// </summary>
    public static List<ObjectString> ScheduleTypes = new List<ObjectString>()
    {
         new ObjectString() { Id = "DONGBOHOCSINH", Name = "Đồng bộ học sinh" },
         new ObjectString() { Id = "DONGBODIEMDANH", Name = "Đồng bộ điểm danh" },
    };
}