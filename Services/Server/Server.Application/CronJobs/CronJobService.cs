using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Server.Application.MasterDatas.A2.Students.V1;
using Server.Application.Services.VTSmart;
using Server.Application.Services.VTSmart.Responses;
using Server.Core.Entities.A2;
using Server.Core.Entities.TA;
using Server.Infrastructure.Datas.MasterData;
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
        //var recurringJobs = JobStorage.Current.GetConnection().GetRecurringJobs();
    }

    public async Task CreateScheduleCronJob(List<ScheduleJob> scheduleLists)
    {
        foreach (var item in scheduleLists)
        {
            var timeSentHour = item.ScheduleTime.HasValue ? item.ScheduleTime.Value.Hours : 0;
            var timeSentMinute = item.ScheduleTime.HasValue ? item.ScheduleTime.Value.Minutes : 0;
            if (item.ScheduleNote == "LAPLICHDONGBO")
            {
                var newCronExpression = item.ScheduleSequential switch
                {
                    "5s" => "*/5 * * * * *",
                    "10s" => "*/10 * * * * *",
                    "20s" => "*/20 * * * * *",
                    "30s" => "*/30 * * * * *",
                    "40s" => "*/40 * * * * *",
                    "50s" => "*/50 * * * * *",

                    "Minutely" => "* * * * *",
                    "5M" => "*/5 * * * *",
                    "10M" => "*/10 * * * *",
                    "20M" => "*/20 * * * *",
                    "30M" => "*/30 * * * *",
                    "40M" => "*/40 * * * *",
                    "50M" => "*/50 * * * *",

                    "Hourly" => "0 * * * *",
                    "Daily" => $"{timeSentMinute} {timeSentHour} * * *",
                    "Weekly" => $"{timeSentMinute} {timeSentHour} * * 0",
                    "Monthly" => $"{timeSentMinute} {timeSentHour} 1 * *",
                    "Yearly" => $"{timeSentMinute} {timeSentHour} 1 1 *",
                    _ => throw new ArgumentException("Invalid ScheduleSequential value")
                };

                if (item.ScheduleType == "DONGBOHOCSINH")
                    await UpdateScheduleSyncStudentCronJob("CronJobSyncSmas[*]" + item.ScheduleType, item.Id, newCronExpression);
                if (item.ScheduleType == "DONGBODIEMDANH")
                    await UpdateScheduleSyncAttendenceCronJob("CronJobSyncSmas[*]" + item.ScheduleType, item.Id, newCronExpression);
            }
        }
    }
    public async Task UpdateScheduleSyncStudentCronJob(string jobId, string sheduleId, string newCronExpression)
    {
        string JobName = jobId + "_" + sheduleId;
        RecurringJob.AddOrUpdate(JobName, () => SyncStudentFromSmas(sheduleId), newCronExpression, TimeZoneInfo.Local);
    }
    public async Task UpdateScheduleSyncAttendenceCronJob(string jobId, string sheduleId, string newCronExpression)
    {
        string JobName = jobId + "_" + sheduleId;
        RecurringJob.AddOrUpdate(JobName, () => SyncAttendenceToSmas(sheduleId), newCronExpression, TimeZoneInfo.Local);
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

            string schoolCode = orgRes.OrganizationCode; // "20186511"
            var res = await _smartService.PostListStudents(schoolCode);

            var logSchedule = new ScheduleJobLog()
            {
                Actived = true,
                CreatedDate = DateTime.Now,
                LastModifiedDate = DateTime.Now,
                OrganizationId = orgRes.Id,
                Logs = jobRes.ScheduleJobName,
                ScheduleJobId = jobRes.Id,
            };

            int count = 0, i = 0;
            if (res.Any())
            {
                count = res.Count();
                foreach (var item in res)
                {
                    i = i + 1;

                    string lastName = !string.IsNullOrWhiteSpace(item.StudentName) ? item.StudentName.Split(' ').Last() : "";

                    var el = new Student()
                    {
                        SyncCode = item.SyncCode,
                        StudentCode = item.StudentCode,
                        EthnicCode = item.StudentCode?.Replace("-", "").Replace(" ", ""),

                        ClassId = item.ClassId,
                        ClassName = item.ClassName,
                        DateOfBirth = item.BirthDay,
                        FullName = item.StudentName,
                        Name = lastName,
                        OrganizationId = orgRes.Id,
                        SchoolCode = orgRes.OrganizationCode,
                    };
                    await _studentService.SaveFromService(el);
                }

                logSchedule.ScheduleJobStatus = true;
                logSchedule.ScheduleLogNote = "Thành công";
                logSchedule.Message = string.Format("Đã đồng bộ {0}/{1} học sinh từ SMAS", i, count);
            }
            else
            {
                logSchedule.ScheduleJobStatus = false;
                logSchedule.ScheduleLogNote = "Thành công";
                logSchedule.Message = string.Format("Không có bản tin nào trả về");
            }


            await _dbContext.ScheduleJobLog.AddAsync(logSchedule);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
        Is_Run_SyncStudentSmasDaily = false;

    }
    public async Task SyncAttendenceToSmas(string sheduleId)
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


            // Lấy dữ liệu theo block gửi qua api
            var datas = await _dbContext.TimeAttendenceEvent.Where(o => o.SchoolCode == orgRes.OrganizationCode && o.EventType != true).OrderBy(o => o.EventTime).Take(30).ToListAsync();
            if (datas.Count == 0)
                return;


            var studentAbs = new List<StudentAbsence>();
            foreach (var item in datas)
            {
                ExtraProperties extra = new ExtraProperties()
                {
                    absenceTime = item.EventTime
                };
                var el = new StudentAbsence()
                {
                    studentCode = item.StudentCode,
                    value = item.ValueAbSent,
                    extraProperties = extra
                };
                studentAbs.Add(el);
            }
            var req = new SyncDataRequest()
            {
                id = Guid.NewGuid().ToString(),
                schoolCode = orgRes.OrganizationCode,
                absenceDate = DateTime.Now,
                section = 0,
                formSendSMS = 1,
                studentCodeType = 2,
                studentAbsenceByDevices = studentAbs,
            };

            //Logger.Warning("SMAS_Req:" + JsonConvert.SerializeObject(req));
            var res = await _smartService.PostSyncAttendence2Smas(req, orgRes.OrganizationCode);
            Logger.Warning("SMAS_Res:" + JsonConvert.SerializeObject(res));

            if (res == null || !res.IsSuccess)
                return;

            datas.ForEach(o => { o.EventType = true; });

            try
            {
                var _listLog = new List<TimeAttendenceSync>();
                foreach (var item in res.Responses)
                {
                    var el = datas.FirstOrDefault(o => o.StudentCode == item.studentCode && o.EventTime == item.extraProperties.absenceTime);
                    if (el == null)
                        continue;

                    var log = new TimeAttendenceSync()
                    {
                        TimeAttendenceEventId = el.Id,
                        SyncStatus = item.status,
                        Message = $"\r\n [{DateTime.Now:dd/MM/yy HH:mm:ss}]: {item.message}",
                        CreatedDate = DateTime.Now,
                        LastModifiedDate = DateTime.Now,
                    };
                    _listLog.Add(log);
                }

                await _dbContext.TimeAttendenceSync.AddRangeAsync(_listLog);
            }
            catch (Exception ext)
            {
                Logger.Error(ext);
            }
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
        Is_Run_SyncStudentSmasDaily = false;

    }

}
