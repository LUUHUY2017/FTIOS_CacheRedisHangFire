using Server.Core.Entities.TA;
using Shared.Core.Commons;

namespace Server.Core.Interfaces.TA.TimeAttendenceEvents;

public interface ITATimeAttendenceEventRepository
{
    Task<Result<TimeAttendenceEvent>> CreateAsync(TimeAttendenceEvent data);
    Task<Result<TimeAttendenceEvent>> UpdateAsync(TimeAttendenceEvent data);
    Task<Result<int>> DeleteAsync(string id);
}


