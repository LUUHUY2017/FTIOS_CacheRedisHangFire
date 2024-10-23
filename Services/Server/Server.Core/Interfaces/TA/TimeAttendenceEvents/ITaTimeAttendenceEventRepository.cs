using Server.Core.Entities.TA;
using Shared.Core.Commons;

namespace Server.Core.Interfaces.TA.TimeAttendenceEvents;

public interface ITATimeAttendenceEventRepository
{
    Task<Result<TA_TimeAttendenceEvent>> CreateAsync(TA_TimeAttendenceEvent data);
    Task<Result<TA_TimeAttendenceEvent>> UpdateAsync(TA_TimeAttendenceEvent data);
    Task<Result<int>> DeleteAsync(string id);
}


