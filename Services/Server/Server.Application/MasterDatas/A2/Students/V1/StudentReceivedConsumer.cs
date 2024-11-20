using AMMS.DeviceData.RabbitMq;
using EventBus.Messages;
using MassTransit;
using Newtonsoft.Json;
using Server.Core.Entities.A2;
using Shared.Core.Loggers;

namespace Server.Application.MasterDatas.A2.Students.V1;
public class StudentReceivedConsumer : IConsumer<RB_DataResponse>
{
    private readonly IEventBusAdapter _eventBusAdapter;
    private readonly Shared.Core.SignalRs.ISignalRClientService _signalRClientService;
    private readonly StudentService _studentService;


    public StudentReceivedConsumer(
        IEventBusAdapter eventBusAdapter
      , Shared.Core.SignalRs.ISignalRClientService signalRClientService,
        StudentService studentService
        )
    {
        _eventBusAdapter = eventBusAdapter;
        _signalRClientService = signalRClientService;
        _studentService = studentService;
    }

    public async Task Consume(ConsumeContext<RB_DataResponse> context)
    {
        try
        {
            // Cập nhật ảnh đồng bộ dữ liệu học sinh
            var dataRes = context.Message;
            if (dataRes != null && dataRes.ReponseType == RB_DataResponseType.UserInfo)
            {
                var dataAte = JsonConvert.DeserializeObject<Student>(dataRes.Content);
                if (dataAte != null)
                    await _studentService.SaveImageFromService(dataAte);
            }

            if (dataRes != null && dataRes.ReponseType == RB_DataResponseType.AttendencePush)
            {
                var dataAte = JsonConvert.DeserializeObject<Student>(dataRes.Content);
                if (dataAte != null)
                    await _studentService.RefreshSyncPage(dataRes.Content);
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
    }
}

