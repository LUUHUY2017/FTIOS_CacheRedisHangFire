namespace Server.Application.CronJobs;

public interface ICronJobService
{
    Task UpdateScheduleSyncSmasCronJob(string jobId, string sheduleId, string newCronExpression);
    Task SyncStudentFromSmas(string sheduleId);
  
}
