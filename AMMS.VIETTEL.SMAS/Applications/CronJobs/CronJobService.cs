using AMMS.VIETTEL.SMAS.Applications.Services.SchoolYearClasses;
using AMMS.VIETTEL.SMAS.Applications.Services.Students.V1;
using AMMS.VIETTEL.SMAS.Applications.Services.VTSmart;
using AMMS.VIETTEL.SMAS.Infratructures.Databases;
using Shared.Core.SignalRs;

namespace AMMS.VIETTEL.SMAS.Applications.CronJobs;

public partial class CronJobService : ICronJobService
{
    private readonly ViettelDbContext _dbContext;
    private readonly SmartService _smartService;
    private readonly StudentService _studentService;
    private readonly IConfiguration _configuration;
    private readonly ISignalRClientService _signalRService;

    private readonly SchoolYearClassService _schoolYearClassService;

    public CronJobService(ViettelDbContext dbContext,
        SmartService smartService,
        StudentService studentService,
        SchoolYearClassService schoolYearClassService,
        IConfiguration configuration,
        ISignalRClientService signalRClientService
        )
    {
        _dbContext = dbContext;
        _smartService = smartService;
        _studentService = studentService;

        _schoolYearClassService = schoolYearClassService;
        _configuration = configuration;
        _signalRService = signalRClientService;

        //var recurringJobs = JobStorage.Current.GetConnection().GetRecurringJobs();
    }


}
