using EventBus.Messages;
using MassTransit;
using Server.Application.MasterDatas.TA.TimeAttendenceEvents.V1;
using Server.Application.Services.VTSmart;
using Server.Application.Services.VTSmart.Responses;
using Shared.Core.Loggers;

namespace Server.Application.MasterDatas.A2.Students.V1;
public class TimeAttendenceSyncSmasConsumer : IConsumer<SyncDataRequest>
{
    private readonly IEventBusAdapter _eventBusAdapter;
    private readonly Shared.Core.SignalRs.ISignalRClientService _signalRClientService;
    private readonly TimeAttendenceEventService _timeAttendenceEventService;
    private readonly SmartService _smartService;


    public TimeAttendenceSyncSmasConsumer(
        IEventBusAdapter eventBusAdapter
      , Shared.Core.SignalRs.ISignalRClientService signalRClientService,
        TimeAttendenceEventService timeAttendenceEventService,
        SmartService smartService
        )
    {
        _eventBusAdapter = eventBusAdapter;
        _signalRClientService = signalRClientService;
        _timeAttendenceEventService = timeAttendenceEventService;
        _smartService = smartService;
    }
    public async Task Consume(ConsumeContext<SyncDataRequest> context)
    {
        try
        {
            var datas = context.Message;
            Logger.Information("Consumer:" + datas);
            await _smartService.PostSyncAttendence2Smas(datas);
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
    }
}


