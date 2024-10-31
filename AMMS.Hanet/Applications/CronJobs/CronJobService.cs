using Hangfire;

namespace AMMS.Hanet.Applications.CronJobs;

public partial class CronJobService : ICronJobService
{
    public CronJobService()
    {

    }
    public async Task Test()
    {
        var newCronExpression = "*/1 * * * *";
        RecurringJob.AddOrUpdate("test", () => Write(), newCronExpression, TimeZoneInfo.Local);
    }
    public void Write()
    {
        Console.WriteLine("Test");
        RecurringJob.RemoveIfExists("test");
    }
}
