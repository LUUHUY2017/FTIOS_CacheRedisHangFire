using Server.Core.Entities.A0;
using Server.Core.Entities.A2;

namespace Server.Application.MasterDatas.A0.AttendanceTimeConfigs.V1.Models;

public class AttendanceTimeConfigResponse
{
    public string? Id { get; set; }
    public string? OrganizationId { get; set; } = "";
    public string? OrganizationName { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }

    public bool? Actived { get; set; }

    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }

    public TimeSpan? LateTime { get; set; }
    public TimeSpan? BreakTime { get; set; }

    public string? Note { get; set; }

    public AttendanceTimeConfigResponse(AttendanceTimeConfig tc, Organization o)
    {
        Id = tc?.Id;
        Name = tc?.Name;
        Type = tc?.Type;
        StartTime = tc?.StartTime;
        EndTime = tc?.EndTime;
        Note = tc?.Note;
        LateTime = tc?.LateTime;
        BreakTime = tc?.BreakTime;
        Actived = tc?.Actived;
        OrganizationId = o?.Id;
        OrganizationName = o?.OrganizationName ?? string.Empty;
    }
}
