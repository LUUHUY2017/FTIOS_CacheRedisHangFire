namespace AMMS.ZkAutoPush.Applications.CronJobs;

public interface ICronJobService
{
    Task DeleteLog();

    Task CheckDeviceOnline();
    Task Test();
    void Write();
}
