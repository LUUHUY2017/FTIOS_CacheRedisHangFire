using AMMS.DeviceData.RabbitMq;
using EventBus.Messages;
using MassTransit;
using Newtonsoft.Json;
using Server.Application.MasterDatas.A2.Devices;
using Server.Core.Entities.A2;
using Shared.Core.Loggers;

namespace Server.Application.MasterDatas.A2.Students.V1;
public class StudentDeviceConsumer : IConsumer<RB_ServerResponse>
{
    private readonly IEventBusAdapter _eventBusAdapter;
    private readonly Shared.Core.SignalRs.ISignalRClientService _signalRClientService;
    private readonly StudentService _studentService;
    private readonly DeviceService _deviceService;


    public StudentDeviceConsumer(
        IEventBusAdapter eventBusAdapter
      , Shared.Core.SignalRs.ISignalRClientService signalRClientService,
        StudentService studentService,
        DeviceService deviceService
        )
    {
        _eventBusAdapter = eventBusAdapter;
        _signalRClientService = signalRClientService;
        _studentService = studentService;
        _deviceService = deviceService;
    }


    public async Task Consume(ConsumeContext<RB_ServerResponse> context)
    {
        try
        {
            //Logger.Information("Consumer:" + dataRes);
            var dataRes = context.Message;

            // Cập nhật trạng thái đồng bộ 1 học sinh trên 1 thiết bị
            if (dataRes != null && dataRes.ReponseType == RB_ServerResponseType.UserInfo)
            {
                var dataAte = JsonConvert.DeserializeObject<TA_PersonInfo>(dataRes.Content);
                if (dataAte != null)
                {
                    await _studentService.SaveStatuSyncDevice(dataRes);
                }
            }

            // Đọc thông tin người dùng và vân tay trên 1 thiết bị
            if (dataRes != null && dataRes.ReponseType == RB_ServerResponseType.DeviceInfo)
            {
                var dataAte = JsonConvert.DeserializeObject<TA_DeviceStatus>(dataRes.Content);
                if (dataAte != null)
                {
                    var de = new Device()
                    {
                        SerialNumber = dataAte.SerialNumber,
                        FaceCount = dataAte.FaceCount,
                        UserCount = dataAte.UserCount,
                        //FingerCount = dataAte.FingerCount,
                    };
                    await _deviceService.UpdateUserFace(de);
                }
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
    }
}
