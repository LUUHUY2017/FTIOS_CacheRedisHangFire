using Shared.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Core.Entities.A2;

[Table("A2_ScheduleJobLog")]
public class A2_ScheduleJobLog : EntityBase
{
    [MaxLength(500)]
    public string? ScheduleJobId { get; set; }
    public bool? ScheduleJobStatus { get; set; }
    [MaxLength(500)]
    public string? Message { get; set; }

    public string? ScheduleJobParams { get; set; }

    [MaxLength(500)]
    public string? ScheduleLogNote { get; set; }
}