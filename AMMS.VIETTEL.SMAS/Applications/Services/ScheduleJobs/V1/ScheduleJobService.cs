using AMMS.VIETTEL.SMAS.Applications.CronJobs;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.ScheduleJobs;
using EventBus.Messages;
using MassTransit;
using Shared.Core.SignalRs;

namespace AMMS.VIETTEL.SMAS.Applications.Services.ScheduleJobs.V1;
public partial class ScheduleJobService
{
    private readonly IEventBusAdapter _eventBusAdapter;
    private readonly ISignalRClientService _signalRClientService;
    private readonly IScheduleJobRepository _scheduleJobRepository;
    private readonly ICronJobService _cronJobService;


    public ScheduleJobService(
        IBus bus,
        IConfiguration configuration,
        IEventBusAdapter eventBusAdapter,
        ISignalRClientService signalRClientService,
        IScheduleJobRepository scheduleJobRepository,
        ICronJobService cronJobService
        )
    {
        _eventBusAdapter = eventBusAdapter;
        _signalRClientService = signalRClientService;
        _scheduleJobRepository = scheduleJobRepository;
        _cronJobService = cronJobService;
    }

    public async Task<bool> CreateScheduleCronJob()
    {
        try
        {
            var result = await _scheduleJobRepository.Gets();
            await _cronJobService.CreateScheduleCronJob(result);
        }
        catch (Exception ex)
        { }
        return true;
    }
}

