using Shared.Core.Entities;

namespace AMMS.VIETTEL.SMAS.Cores.Entities.A2;

public class AttendanceTimeConfig : EntityBase
{
    public string? Name { get; set; }
    public string? Type { get; set; }
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }

    public TimeSpan? LateTime { get; set; }
    public TimeSpan? BreakTime { get; set; }
    public string? Note { get; set; }
}
