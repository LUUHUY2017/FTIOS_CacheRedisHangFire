using AMMS.Notification.Commons;
using AMMS.Notification.Workers.Emails;
using DocumentFormat.OpenXml.Drawing;
using Hangfire;
using Hangfire.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Server.Application.MasterDatas.A2.Students.V1;
using Server.Application.MasterDatas.A2.Students.V1.Model;
using Server.Application.Services.VTSmart;
using Server.Core.Entities.A2;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.Emails.V1.Commons;
using Shared.Core.Loggers;
using Shared.Core.SignalRs;

namespace Server.Application.CronJobs;

public class CronJobService : ICronJobService
{
    private readonly IMasterDataDbContext _dbContext;
    private readonly SmartService _smartService;
    private readonly StudentService _studentService;
    private readonly IConfiguration _configuration;
    private readonly ISignalRClientService _signalRService;

    public CronJobService(IMasterDataDbContext dbContext,
        SmartService smartService,
        StudentService studentService,
        IConfiguration configuration,
        ISignalRClientService signalRClientService)
    {
        _dbContext = dbContext;
        _smartService = smartService;
        _studentService = studentService;
        _configuration = configuration;
        _signalRService = signalRClientService;
    }
    public async Task CreateScheduleSendMailCronJob(List<ScheduleJob> scheduleLists)
    {
        foreach (var item in scheduleLists)
        {
            var timeSentHour = item.ScheduleTime.HasValue ? item.ScheduleTime.Value.Hours : 0;
            var timeSentMinute = item.ScheduleTime.HasValue ? item.ScheduleTime.Value.Hours : 0;
            var newCronExpression = "0 * * * *";
            if (item.ScheduleNote == "LAPLICHDONGBO")
            {
                newCronExpression = item.ScheduleSequential switch
                {
                    "Minutely" => "* * * * *",
                    "Hourly" => "0 * * * *",
                    "Daily" => $"{timeSentMinute} {timeSentHour} * * *",
                    "Weekly" => $"{timeSentMinute} {timeSentHour} * * 0",
                    "Monthly" => $"{timeSentMinute} {timeSentHour} 1 * *",
                    "Yearly" => $"{timeSentMinute} {timeSentHour} 1 1 *",
                    _ => throw new ArgumentException("Invalid ScheduleSequential value")
                };
                await UpdateSchedulCronJob("ScheduleJob" + item.ScheduleSequential, item.Id, newCronExpression);
            }
        }
    }

    public async Task UpdateSchedulCronJob(string jobId, string sheduleId, string newCronExpression)
    {
        //var recurringJobs = JobStorage.Current.GetConnection().GetRecurringJobs();
        string JobName = jobId + "_" + sheduleId;
        RecurringJob.AddOrUpdate(JobName, () => SyncStudentFromSmas(sheduleId), newCronExpression, TimeZoneInfo.Local);
    }

    public async Task RemoveScheduleCronJob(string jobId, string sheduleId)
    {
        string JobName = jobId + "_" + sheduleId;
        RecurringJob.RemoveIfExists(JobName);
    }


    static bool Is_Run_SyncStudentSmasDaily = false;
    public async Task SyncStudentFromSmas(string sheduleId)
    {
        DateTime now = DateTime.Now;
        try
        {
            var jobRes = await _dbContext.ScheduleJob.FirstOrDefaultAsync(o => o.Actived == true && o.Id == sheduleId);
            if (jobRes == null)
                return;

            var orgRes = await _dbContext.Organization.FirstOrDefaultAsync(o => o.Id == jobRes.OrganizationId && o.Actived == true);
            if (orgRes == null)
                return;

            string provinceCode = orgRes.ProvinceCode; // 20 tỉnh Lạng Sơn
            string schoolCode = orgRes.OrganizationCode; // "20186511"
            string schoolYearCode = "2024-2025";
            var res = await _smartService.PostListStudents(provinceCode, schoolCode, schoolYearCode);
            if (res.Any())
            {
                foreach (var item in res)
                {
                    var el = new DtoStudentRequest()
                    {
                        StudentCode = item.StudentCode,
                        ClassId = item.ClassId,
                        ClassName = item.ClassName,
                        DateOfBirth = item.BirthDay,
                        FullName = item.StudentName
                    };
                    _studentService.SaveFromService(el);
                }
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
        Is_Run_SyncStudentSmasDaily = false;

    }
}
