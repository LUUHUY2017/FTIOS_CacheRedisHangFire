using AMMS.VIETTEL.SMAS.Cores.Entities.A2;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.ScheduleJobs.Reponses;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.ScheduleJobs.Requests;
using Shared.Core.Commons;

namespace AMMS.VIETTEL.SMAS.Cores.Interfaces.ScheduleJobs;

public interface IScheduleJobRepository
{
    Task<Result<ScheduleJob>> GetById(string id);
    Task<List<ScheduleJobReportResponse>> GetAlls(ScheduleJobFilterRequest request);
    Task<List<ScheduleJob>> Gets(bool actived = true);
    Task<Result<ScheduleJob>> UpdateAsync(ScheduleJob data);
    Task<Result<ScheduleJob>> ActiveAsync(ActiveRequest data);
    Task<Result<ScheduleJob>> InActiveAsync(InactiveRequest data);

}