using Server.Core.Entities;

namespace Server.Application.CronJobs;

public interface IConJobService
{
    Task CheckDeviceOnline();
    void CreateScheduleSendMailCronJob(List<ScheduleSendMail> scheduleLists);
    void CreateScheduleReportInOutCronJob(List<ScheduleSendMail> scheduleLists);
    void UpdateScheduleSendMailCronJob(string jobId, int sheduleId, string newCronExpression);
    void UpdateScheduleReportInOutCronJob(string jobId, int sheduleId, string newCronExpression);
    void POC_Report_ScheduleSendMailReportDaily(int sheduleId);
    void POC_Report_ScheduleSendMailReportMonthly(int sheduleId);
    void Device_Warning_ScheduleSendMailHourly(int sheduleId);
    void POC_Report_ScheduleReSendMailReport();

    void LayDuLieuDeGuiEmailCanhBaoHomNaySoVoiHomQua(); 
}
