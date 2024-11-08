using Server.Core.Entities.TA;
using Shared.Core.Commons;

namespace Server.Core.Interfaces.TA.TimeAttendenceSyncs;

public interface ITATimeAttendenceSyncRepository
{
    Task<Result<TimeAttendenceSync>> CreateAsync(TimeAttendenceSync data);
    Task<Result<TimeAttendenceSync>> UpdateAsync(TimeAttendenceSync data);
    Task<Result<TimeAttendenceSync>> UpdateStatusAsync(TimeAttendenceSync data);
    Task<Result<int>> DeleteAsync(string id);
}



