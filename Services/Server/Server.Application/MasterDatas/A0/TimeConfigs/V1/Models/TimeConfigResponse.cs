using Server.Core.Entities.A0;
using Server.Core.Entities.A2;

namespace Server.Application.MasterDatas.A0.TimeConfigs.V1.Models;

public class TimeConfigResponse
{
    public string? Id { get; set; }
    public string? OrganizationId { get; set; }
    public string? OrganizationName { get; set; }

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

    public TimeConfigResponse(A0_TimeConfig tc, A2_Organization o)
    {
         Id = tc?.Id;
         OrganizationId = tc?.OrganizationId;
         OrganizationName = o?.OrganizationName ?? string.Empty;

         MorningStartTime = tc?.MorningStartTime;
         MorningEndTime = tc?.MorningEndTime;
         MorningLateTime = tc?.MorningLateTime;
         MorningBreakTime = tc?.MorningBreakTime;

         AfternoonStartTime = tc?.AfternoonStartTime;
         AfternoonEndTime = tc?.AfternoonEndTime;
         AfternoonLateTime = tc?.AfternoonLateTime;
         AfternoonBreakTime = tc?.AfternoonBreakTime;

         EveningStartTime = tc?.EveningStartTime;
         EveningEndTime = tc?.EveningEndTime;
         EveningLateTime = tc?.EveningLateTime;
         EveningBreakTime = tc?.EveningBreakTime;
}
}
