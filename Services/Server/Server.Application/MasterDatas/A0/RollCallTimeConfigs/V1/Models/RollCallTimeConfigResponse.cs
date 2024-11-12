using Server.Core.Entities.A0;
using Server.Core.Entities.A2;

namespace Server.Application.MasterDatas.A0.RollCallTimeConfigs.V1.Models;

public class RollCallTimeConfigResponse
{
    public string? Id { get; set; }
    public string? OrganizationId { get; set; } = "";
    public string? OrganizationName { get; set; }
    public string? RollCallName { get; set; }

    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }

    public string? Note { get; set; }

    public RollCallTimeConfigResponse(RollCallTimeConfig tc, Organization o)
    {
        Id = tc?.Id;
        RollCallName = tc?.RollCallName;
        StartTime = tc?.StartTime;
        EndTime = tc?.EndTime;
        Note = tc?.Note;
        OrganizationId = o?.Id;
        OrganizationName = o?.OrganizationName ?? string.Empty;
    }
}
