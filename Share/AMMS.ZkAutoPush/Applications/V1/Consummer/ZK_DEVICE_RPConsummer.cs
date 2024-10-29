using AMMS.ZkAutoPush.Data;
using MassTransit;

namespace AMMS.ZkAutoPush.Applications.V1.Consummer;

public class ZK_DEVICE_RPConsummer : IConsumer<ZK_DEVICE_RP>
{
    ZK_DEVICE_RPService _zK_RP_DATAService;
    public ZK_DEVICE_RPConsummer(ZK_DEVICE_RPService zK_RP_DATAService)
    {
        _zK_RP_DATAService = zK_RP_DATAService;
    }
    public async Task Consume(ConsumeContext<ZK_DEVICE_RP> context)
    {
        //Xử lý chèn vào CSDL nên sử dụng IMediator
        if(context== null || context.Message ==null) return;

        await _zK_RP_DATAService.ProcessData(context.Message);
    }
}
