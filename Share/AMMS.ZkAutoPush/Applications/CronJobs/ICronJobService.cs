namespace AMMS.ZkAutoPush.Applications.CronJobs;

public interface ICronJobService
{
    Task CheckDeviceOnline();
    Task Test();
    void Write();
}
