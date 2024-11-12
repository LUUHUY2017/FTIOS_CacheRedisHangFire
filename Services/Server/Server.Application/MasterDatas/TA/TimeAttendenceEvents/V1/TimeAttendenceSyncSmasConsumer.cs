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

            //var res = await _smartService.PostSyncAttendence2Smas(datas, datas.schoolCode);

            //if (res != null)
            //{
            //    var item = new TimeAttendenceSync() { Id = datas.id, };
            //    if (res.IsSuccess)
            //    {
            //        string response = JsonConvert.SerializeObject(res);
            //        item.SyncStatus = res.Responses[0].status;
            //        item.Message = res.Responses[0].message;
            //        item.ParamResponses = response;
            //    }
            //    else
            //    {
            //        item.SyncStatus = res.IsSuccess;
            //        item.Message = res.Message;
            //    }
            //    await _timeAttendenceEventService.SaveStatuSyncSmas(item);
            //}
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
    }
}


