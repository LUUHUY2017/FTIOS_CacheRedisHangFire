using AMMS.DeviceData.RabbitMq;
using EventBus.Messages;
using MassTransit;
using Newtonsoft.Json;
using Server.Application.MasterDatas.TA.TimeAttendenceEvents.V1;
using Shared.Core.Loggers;

namespace Server.Application.MasterDatas.A2.Students.V1;
public class TimeAttendenceEventConsumer : IConsumer<RB_DataResponse>
{
    private readonly IEventBusAdapter _eventBusAdapter;
    private readonly Shared.Core.SignalRs.ISignalRClientService _signalRClientService;
    private readonly TimeAttendenceEventService _timeAttendenceEventService;
    private readonly StudentService _studentService;


    public TimeAttendenceEventConsumer(
        IEventBusAdapter eventBusAdapter
      , Shared.Core.SignalRs.ISignalRClientService signalRClientService,
        TimeAttendenceEventService timeAttendenceEventService,
        StudentService studentService
        )
    {
        _eventBusAdapter = eventBusAdapter;
        _signalRClientService = signalRClientService;
        _timeAttendenceEventService = timeAttendenceEventService;
        _studentService = studentService;
    }
    public async Task Consume(ConsumeContext<RB_DataResponse> context)
    {
        try
        {
            List<TA_AttendenceHistory> list = new List<TA_AttendenceHistory>();
            var dataRes = context.Message;
            //Logger.Information("Consumer:" + dataRes);

            if (dataRes != null && dataRes.ReponseType == RB_DataResponseType.AttendenceHistory)
            {
                var dataAte = JsonConvert.DeserializeObject<TA_AttendenceHistory>(dataRes.Content);
                if (dataAte != null)
                {
                    list.Add(dataAte);
                    await _timeAttendenceEventService.ProcessAtendenceData(list);
                }
            }

            if (dataRes != null && dataRes.ReponseType == RB_DataResponseType.AttendenceImage)
            {
                var dataAte = JsonConvert.DeserializeObject<TA_AttendenceImage>(dataRes.Content);
                if (dataAte != null)
                {
                    //await _timeAttendenceEventService.ProcessAttendenceImage(dataAte.ImageBase64, dataAte.AttendenceHistoryId);
                }
            }

            if (dataRes != null && dataRes.ReponseType == RB_DataResponseType.UserInfo)
            {
                var dataAte = JsonConvert.DeserializeObject<TA_PersonInfo>(dataRes.Content);
                if (dataAte != null)
                {
                    await _studentService.SaveImagePerson(dataAte);
                }
            }
        }
        catch (Exception ex)
        {
            Logger.Warning(ex.Message);
        }
    }
}

