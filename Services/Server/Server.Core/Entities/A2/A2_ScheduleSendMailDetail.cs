using Shared.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Core.Entities.A2;

[Table("ScheduleSendMailDetail")]
public class A2_ScheduleSendMailDetail : EntityBase
{

    /// <summary>
    /// Tên lịch báo cáo
    /// </summary>
    public string? ScheduleId { get; set; }

    /// <summary>
    /// Email nhận báo cáo
    /// </summary>
    public string ScheduleEmail { get; set; }
    /// <summary>
    /// Tên người nhận báo cáo
    /// </summary>
    public string ScheduleUserName { get; set; }

}
