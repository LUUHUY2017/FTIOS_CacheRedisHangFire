using Shared.Core.Entities;

namespace Server.Core.Entities.A0;

public class AttendanceTimeConfig : EntityBase
{
    public string? RollCallName { get; set; }
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }

    public string? Note { get; set; }
}
