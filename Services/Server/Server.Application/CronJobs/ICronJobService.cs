using Server.Core.Entities.A2;

namespace Server.Application.CronJobs;

public interface ICronJobService
{
    Task CreateScheduleSendMailCronJob(List<ScheduleJob> scheduleLists);
    Task UpdateSchedulCronJob(string jobId, string sheduleId, string newCronExpression);
    Task RemoveScheduleCronJob(string jobId, string sheduleId );
    Task SyncStudentFromSmas(string sheduleId);
  
}
