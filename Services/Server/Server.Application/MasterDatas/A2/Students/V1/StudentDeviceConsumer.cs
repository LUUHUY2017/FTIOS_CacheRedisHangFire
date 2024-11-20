using AMMS.DeviceData.RabbitMq;
using EventBus.Messages;
using MassTransit;
using Shared.Core.Loggers;

namespace Server.Application.MasterDatas.A2.Students.V1;
public class StudentDeviceConsumer : IConsumer<RB_ServerResponse>
{
    private readonly IEventBusAdapter _eventBusAdapter;
    private readonly Shared.Core.SignalRs.ISignalRClientService _signalRClientService;
    private readonly StudentService _studentService;


    public StudentDeviceConsumer(
        IEventBusAdapter eventBusAdapter
      , Shared.Core.SignalRs.ISignalRClientService signalRClientService,
        StudentService studentService
        )
    {
        _eventBusAdapter = eventBusAdapter;
        _signalRClientService = signalRClientService;
        _studentService = studentService;
    }


    public async Task Consume(ConsumeContext<RB_ServerResponse> context)
    {
        try
        {
            // Cập nhật trạng thái đồng bộ dữ liệu học sinh
            var dataRes = context.Message;
            var retval = await _studentService.SaveStatuSyncDevice(dataRes);
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
    }
}

