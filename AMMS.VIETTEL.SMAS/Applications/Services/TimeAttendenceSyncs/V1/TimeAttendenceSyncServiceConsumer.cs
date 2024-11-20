using AMMS.DeviceData.RabbitMq;
using AMMS.VIETTEL.SMAS.Applications.Services.VTSmart.Responses;
using EventBus.Messages;
using MassTransit;
using Newtonsoft.Json;
using Shared.Core.Loggers;
using Shared.Core.SignalRs;

namespace AMMS.VIETTEL.SMAS.Applications.Services.TimeAttendenceSyncs.V1;
public class TimeAttendenceSyncServiceConsumer : IConsumer<RB_DataResponse>
{
    private readonly IEventBusAdapter _eventBusAdapter;
    private readonly ISignalRClientService _signalRClientService;
    private readonly TimeAttendenceSyncService _timeAttendenceSyncService;


    public TimeAttendenceSyncServiceConsumer(
        IEventBusAdapter eventBusAdapter
      , ISignalRClientService signalRClientService,
        TimeAttendenceSyncService timeAttendenceSyncService
        )
    {
        _eventBusAdapter = eventBusAdapter;
        _signalRClientService = signalRClientService;
        _timeAttendenceSyncService = timeAttendenceSyncService;
    }
    public async Task Consume(ConsumeContext<RB_DataResponse> context)
    {
        try
        {
            var dataRes = context.Message;
            //Logger.Information("Consumer:" + dataRes);
            if (dataRes != null && dataRes.ReponseType == RB_DataResponseType.AttendencePush)
            {
                var dataAte = JsonConvert.DeserializeObject<SyncDataRequest>(dataRes.Content);
                if (dataAte != null)
                    await _timeAttendenceSyncService.ProcessAtendenceData(dataAte);
            }
        }
        catch (Exception ex)
        {
            Logger.Warning(ex.Message);
        }
    }
}

