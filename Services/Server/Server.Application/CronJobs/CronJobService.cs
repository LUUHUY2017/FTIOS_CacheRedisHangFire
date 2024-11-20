using AMMS.Notification.Workers.Emails;
using Microsoft.Extensions.Configuration;
using Server.Application.MasterDatas.A2.DashBoards.V1;
using Server.Application.MasterDatas.A2.DeviceNotifications.V1;
using Server.Application.MasterDatas.A2.MonitorDevices.V1;
using Server.Application.MasterDatas.A2.SchoolYearClasses.V1;
using Server.Application.MasterDatas.A2.Students.V1;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.SignalRs;

namespace Server.Application.CronJobs;

public partial class CronJobService : ICronJobService
{
    private readonly IMasterDataDbContext _dbContext;
    //private readonly SmartService _smartService;
    private readonly StudentService _studentService;
    private readonly IConfiguration _configuration;
    private readonly ISignalRClientService _signalRService;

    private readonly SchoolYearClassService _schoolYearClassService;
    private readonly SendEmailMessageService1 _sendEmailMessageService1;
    private readonly DeviceReportService _deviceReportService;
    private readonly MonitorDeviceService _monitorDeviceService;
    private readonly DashBoardService _dashBoardService;

    public CronJobService(IMasterDataDbContext dbContext,
        //SmartService smartService,
        StudentService studentService,
        SchoolYearClassService schoolYearClassService,
        IConfiguration configuration,
        ISignalRClientService signalRClientService,
        SendEmailMessageService1 sendEmailMessageService1,
        DeviceReportService deviceReportService,
        MonitorDeviceService monitorDeviceService,
        DashBoardService dashBoardService
        )
    {
        _dbContext = dbContext;
        //_smartService = smartService;
        _studentService = studentService;
        _schoolYearClassService = schoolYearClassService;
        _configuration = configuration;
        _signalRService = signalRClientService;
        _sendEmailMessageService1 = sendEmailMessageService1;
        _deviceReportService = deviceReportService;
        _monitorDeviceService = monitorDeviceService;
        _dashBoardService = dashBoardService;
        //var recurringJobs = JobStorage.Current.GetConnection().GetRecurringJobs();
    }

}
