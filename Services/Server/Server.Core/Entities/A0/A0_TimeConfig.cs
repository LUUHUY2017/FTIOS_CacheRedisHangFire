using Shared.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Core.Entities.A0;

[Table("TimeConfig")]

public class A0_TimeConfig : EntityBase
{
    public string? OrganizationId { get; set; }

    public TimeSpan? MorningStartTime { get; set; }
    public TimeSpan? MorningEndTime { get; set; }
    public TimeSpan? MorningLateTime { get; set; }
    public TimeSpan? MorningBreakTime { get; set; }

    public TimeSpan? AfternoonStartTime { get; set; }
    public TimeSpan? AfternoonEndTime { get; set; }
    public TimeSpan? AfternoonLateTime { get; set; }
    public TimeSpan? AfternoonBreakTime { get; set; }

    public TimeSpan? EveningStartTime { get; set; }
    public TimeSpan? EveningEndTime { get; set; }
    public TimeSpan? EveningLateTime { get; set; }
    public TimeSpan? EveningBreakTime { get; set; }

    public string? Note { get; set; }
}
