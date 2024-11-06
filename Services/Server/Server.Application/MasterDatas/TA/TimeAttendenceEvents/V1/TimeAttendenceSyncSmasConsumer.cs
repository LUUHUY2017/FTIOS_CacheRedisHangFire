using EventBus.Messages;
using MassTransit;
using Newtonsoft.Json;
using Server.Application.MasterDatas.TA.TimeAttendenceEvents.V1;
using Server.Application.Services.VTSmart;
using Server.Application.Services.VTSmart.Responses;
using Server.Core.Entities.TA;
using Shared.Core.Loggers;
using Shared.Core.SignalRs;

namespace Server.Application.MasterDatas.A2.Students.V1;
public class TimeAttendenceSyncSmasConsumer : IConsumer<SyncDataRequest>
{
    private readonly IEventBusAdapter _eventBusAdapter;
    private readonly ISignalRClientService _signalRClientService;
    private readonly TimeAttendenceEventService _timeAttendenceEventService;
    private readonly SmartService _smartService;


    public TimeAttendenceSyncSmasConsumer(
        IEventBusAdapter eventBusAdapter,
        ISignalRClientService signalRClientService,
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
            var res = await _smartService.PostSyncAttendence2Smas(datas);

            var item = new TA_TimeAttendenceSync()
            {
                Id = datas.Id,
            };

            if (res != null && res.isSuccess)
            {
                string response = JsonConvert.SerializeObject(res);
                item.SyncStatus = res.responses[0].status;
                item.Message = res.responses[0].message;
                item.ParamResponses = response;
            }
            else
            {
                item.SyncStatus = res.isSuccess;
                item.Message = res.message;
            }
            var retval = await _timeAttendenceEventService.SaveStatuSyncSmas(item);
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
    }
}


