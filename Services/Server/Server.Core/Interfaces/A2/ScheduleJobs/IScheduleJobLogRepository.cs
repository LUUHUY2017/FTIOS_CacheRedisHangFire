using Server.Core.Entities.A2;
using Shared.Core.Commons;

namespace Server.Core.Interfaces.A2.ScheduleJobs;

public interface IScheduleJobLogRepository
{
    Task<Result<ScheduleJobLog>> GetById(string id);
    Task<List<ScheduleJobLog>> GetByScheduleJobId(string scheduleJobId);
    Task<List<ScheduleJobLog>> Gets(bool actived = true);

    Task<Result<ScheduleJobLog>> UpdateAsync(ScheduleJobLog data);
    Task<Result<ScheduleJobLog>> ActiveAsync(ActiveRequest data);
    Task<Result<ScheduleJobLog>> InActiveAsync(InactiveRequest data);

}