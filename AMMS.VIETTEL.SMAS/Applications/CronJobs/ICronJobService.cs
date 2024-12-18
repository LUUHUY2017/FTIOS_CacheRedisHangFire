﻿using AMMS.VIETTEL.SMAS.Cores.Entities.A2;

namespace AMMS.VIETTEL.SMAS.Applications.CronJobs;

public interface ICronJobService
{
    Task<string> CheckTimeSyncAttendence(string orgId);
    Task CreateScheduleCronJob(List<ScheduleJob> scheduleLists);
    Task UpdateScheduleSyncStudentCronJob(string jobId, string sheduleId, string newCronExpression);
    Task RemoveScheduleCronJob(string jobId, string sheduleId);
    Task SyncStudentFromSmas(string sheduleId);
    Task SyncAttendenceToSmas(string sheduleId);

}
