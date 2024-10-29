using AMMS.DeviceData.RabbitMq;
using AMMS.ZkAutoPush.Data;
using MassTransit;
using Newtonsoft.Json;
using Shared.Core.Loggers;

namespace AMMS.ZkAutoPush.Applications.V1.Consummer;

public class ZK_TA_DataConsummer : IConsumer<ZK_TA_DATA>
{
    ZK_TA_DataService _zK_TA_DataService;
    public ZK_TA_DataConsummer(ZK_TA_DataService zK_TA_DataService)
    {
        _zK_TA_DataService = zK_TA_DataService;
    }
    public async Task Consume(ConsumeContext<ZK_TA_DATA> context)
    {
        try
        {
            if (context == null || context.Message == null)
                return;

            ZK_TA_DATA data = context.Message;

            if (data == null)
                return;
            await _zK_TA_DataService.AddTA_Data(data);
        }
        catch (Exception ex)
        {
            Logger.Warning(ex.Message);
        }
    }
}
