using Server.Core.Entities.TA;
using Shared.Core.Commons;

namespace Server.Core.Interfaces.TA.TimeAttendenceSyncs;

public interface ITATimeAttendenceSyncRepository
{
    Task<Result<TA_TimeAttendenceSync>> CreateAsync(TA_TimeAttendenceSync data);
    Task<Result<TA_TimeAttendenceSync>> UpdateAsync(TA_TimeAttendenceSync data);
    Task<Result<TA_TimeAttendenceSync>> UpdateStatusAsync(TA_TimeAttendenceSync data);
    Task<Result<int>> DeleteAsync(string id);
}



