using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.ScheduleJobs.Reponses;
using Server.Core.Interfaces.A2.ScheduleJobs.Requests;
using Shared.Core.Commons;

namespace Server.Core.Interfaces.A2.ScheduleJobs;

public interface IScheduleJobRepository
{
    Task<Result<ScheduleJob>> GetById(string id);
    Task<List<ScheduleJobReportResponse>> GetAlls(ScheduleJobFilterRequest request);
    Task<Result<ScheduleJob>> UpdateAsync(ScheduleJob data);
    Task<Result<ScheduleJob>> ActiveAsync(ActiveRequest data);
    Task<Result<ScheduleJob>> InActiveAsync(InactiveRequest data);

}