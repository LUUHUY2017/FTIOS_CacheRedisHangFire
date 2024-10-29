using AMMS.Hanet.Data.Response;
using MassTransit;

namespace AMMS.Hanet.Applications.V1.Consummer;

public class HANET_DEVICE_PUSHConsummer : IConsumer<Hanet_Device_Data>
{
    public HANET_DEVICE_PUSHConsummer()
    {
    }
    public async Task Consume(ConsumeContext<Hanet_Device_Data> context)
    {
        //Xử lý chèn vào CSDL nên sử dụng IMediator
        if (context == null || context.Message == null) return;

        //await _Hanet_RP_DATAService.ProcessData(context.Message);
    }
}
