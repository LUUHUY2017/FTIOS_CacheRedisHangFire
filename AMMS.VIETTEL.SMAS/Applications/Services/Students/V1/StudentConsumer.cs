using AMMS.DeviceData.RabbitMq;
using EventBus.Messages;
using MassTransit;
using Shared.Core.Loggers;
using Shared.Core.SignalRs;

namespace AMMS.VIETTEL.SMAS.Applications.Services.Students.V1;
public class StudentConsumer : IConsumer<RB_ServerResponse>
{
    private readonly IEventBusAdapter _eventBusAdapter;
    private readonly ISignalRClientService _signalRClientService;
    private readonly StudentService _studentService;


    public StudentConsumer(
        IEventBusAdapter eventBusAdapter
      , ISignalRClientService signalRClientService,
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
            var dataRes = context.Message;
            // Cập nhật trạng thái đồng bộ dữ liệu học sinh
            var retval = await _studentService.SaveStatuSyncDevice(dataRes);

            //if (_signalRClientService.Connection != null && _signalRClientService.Connection.State == HubConnectionState.Connected)
            //{
            //    _signalRClientService.Connection.SendAsync("MonitorDevice", xxx);
            //}
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
    }
}

