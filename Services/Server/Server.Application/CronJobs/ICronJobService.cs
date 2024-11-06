namespace Server.Application.CronJobs;

public interface ICronJobService
{
    Task SyncStudentFromSmas();
    void Write();
}
