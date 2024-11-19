namespace AMMS.Hanet.Applications.CronJobs;

public interface ICronJobService
{
    Task CheckDeviceOnline();
    Task DeleteLog();
    Task Test();
    void Write();
}
