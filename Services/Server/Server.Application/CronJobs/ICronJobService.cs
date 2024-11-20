using Server.Core.Entities.A2;

namespace Server.Application.CronJobs;

public interface ICronJobService
{
    //Task CreateScheduleCronJob(List<ScheduleJob> scheduleLists);
    //Task UpdateScheduleSyncStudentCronJob(string jobId, string sheduleId, string newCronExpression);
    //Task RemoveScheduleCronJob(string jobId, string sheduleId );
    //Task SyncStudentFromSmas(string sheduleId);
    //Task SyncAttendenceToSmas(string sheduleId);

    Task Device_Warning_ScheduleSendMail(string sheduleId);
    Task CreateDeviceStatusWarningCronJob(List<ScheduleSendMail> scheduleLists);
    Task UpdateDeviceStatusWarningCronJob(string jobId, string sheduleId, string newCronExpression);
    Task CheckDataReception();

}
