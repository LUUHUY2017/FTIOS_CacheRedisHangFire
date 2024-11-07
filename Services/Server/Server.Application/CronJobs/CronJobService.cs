using AMMS.Notification.Commons;
using AMMS.Notification.Workers.Emails;
using Hangfire;
using Hangfire.Storage;
using Microsoft.Extensions.Configuration;
using Server.Application.MasterDatas.A2.Students.V1;
using Server.Application.MasterDatas.A2.Students.V1.Model;
using Server.Application.Services.VTSmart;
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


    public async Task UpdateScheduleSyncSmasCronJob(string jobId, string sheduleId, string newCronExpression)
    {
        //var recurringJobs = JobStorage.Current.GetConnection().GetRecurringJobs();
        string JobName = jobId + "_" + sheduleId;

        //if (jobId == "ScheduleSendMailReportDaily")
        {
            RecurringJob.AddOrUpdate(JobName, () => SyncStudentFromSmas(sheduleId), newCronExpression, TimeZoneInfo.Local);
        }

    }

    static bool Is_Run_SyncStudentSmasDaily = false;
    public async Task SyncStudentFromSmas(string sheduleId)
    {
        DateTime now = DateTime.Now;
        try
        {
            //var datas = await _dbContext.ScheduleSendMail.Where(o => o.Actived == true && o.ScheduleSequentialSending == "Daily").ToListAsync();
            //if (datas.Any())
            //{
            //foreach (var item in datas)
            //{
            DateTime date = DateTime.Now;
            // Nếu dữ liệu lấy ngày hôm trước -1days
            //if (item.ScheduleDataCollect != "Current")
            //    date = DateTime.Now.AddDays(-1);

            //var timeNow = now.Hour;
            //var timeSend = item.ScheduleTimeSend.Value.Hours;
            //var start_date = new DateTime(date.Year, date.Month, date.Day, 00, 00, 00);
            //var end_date = new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);

            //// Reset lại thời gian gửi
            //if (now.Hour == 0)
            //    item.ReportSent = false;
            //// Kiểm tra đúng thời gian cấu hình sẽ gửi
            //if (timeNow != timeSend)
            //    continue;
            //// Nếu thời gian chạy ngắn màn bản tin check đã gửi rồi thì không gửi nữa
            //if (timeNow == timeSend && item.ReportSent == true)
            //    continue;

            //if (sheduleId != item.Id)
            //    continue;

            //}
            //}

            string provinceCode = "20";
            string schoolCode = "20186511";
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
