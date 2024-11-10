using AMMS.VIETTEL.SMAS.Applications.Services.Students.V1;
using AMMS.VIETTEL.SMAS.Applications.Services.VTSmart;
using AMMS.VIETTEL.SMAS.Applications.Services.VTSmart.Responses;
using AMMS.VIETTEL.SMAS.Cores.Entities.A2;
using AMMS.VIETTEL.SMAS.Infratructures.Databases;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Shared.Core.Loggers;
using Shared.Core.SignalRs;

namespace AMMS.VIETTEL.SMAS.Applications.CronJobs;

public class CronJobService : ICronJobService
{
    private readonly IViettelDbContext _dbContext;
    private readonly SmartService _smartService;
    private readonly StudentService _studentService;
    private readonly IConfiguration _configuration;
    //private readonly ISignalRClientService _signalRService;

    public CronJobService(IViettelDbContext dbContext,
        SmartService smartService,
        StudentService studentService,
        IConfiguration configuration
        //ISignalRClientService signalRClientService
        )
    {
        _dbContext = dbContext;
        _smartService = smartService;
        _studentService = studentService;
        _configuration = configuration;
        //_signalRService = signalRClientService;
        //var recurringJobs = JobStorage.Current.GetConnection().GetRecurringJobs();
    }

    public async Task CreateScheduleCronJob(List<ScheduleJob> scheduleLists)
    {
        foreach (var item in scheduleLists)
        {
            var timeSentHour = item.ScheduleTime.HasValue ? item.ScheduleTime.Value.Hours : 0;
            var timeSentMinute = item.ScheduleTime.HasValue ? item.ScheduleTime.Value.Hours : 0;
            if (item.ScheduleNote == "LAPLICHDONGBO")
            {
                var newCronExpression = item.ScheduleSequential switch
                {
                    "Minutely" => "* * * * *",
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
                        FullName = item.StudentName,
                        OrganizationId = orgRes.Id,
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
            var datas = await _dbContext.TimeAttendenceEvent.Where(o => o.SchoolCode == orgRes.OrganizationCode && o.EventType != true).OrderBy(o => o.EventTime).Take(20).ToListAsync();
            var studentAbs = new List<StudentAbsence>();
            foreach (var item in datas)
            {
                var el = new StudentAbsence()
                {
                    StudentCode = item.StudentCode,
                    Value = item.ValueAbSent
                };
                studentAbs.Add(el);
            }
            var req = new SyncDataRequest()
            {
                Id = Guid.NewGuid().ToString(),
                SchoolCode = orgRes.OrganizationCode,
                AbsenceDate = DateTime.Now,
                Section = 0,
                FormSendSMS = 1,
                StudentAbsences = studentAbs,
            };
            var res = await _smartService.PostSyncAttendence2Smas(req, orgRes.OrganizationCode);
            if (res != null)
            {
                if (res.IsSuccess)
                {
                    datas.ForEach(o => { o.EventType = true; });
                    await _dbContext.SaveChangesAsync();
                }
                //var item = new TimeAttendenceSync() { Id = datas.Id, };
                //if (res.IsSuccess)
                //{
                //    string response = JsonConvert.SerializeObject(res);
                //    item.SyncStatus = res.Responses[0].status;
                //    item.Message = res.Responses[0].message;
                //    item.ParamResponses = response;
                //}
                //else
                //{
                //    item.SyncStatus = res.IsSuccess;
                //    item.Message = res.Message;
                //}
                //await _timeAttendenceEventService.SaveStatuSyncSmas(item);
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
        Is_Run_SyncStudentSmasDaily = false;

    }

}
