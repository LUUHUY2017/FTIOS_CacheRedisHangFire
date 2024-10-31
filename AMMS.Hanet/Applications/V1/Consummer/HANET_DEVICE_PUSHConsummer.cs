using AMMS.Hanet.Applications.V1.Service;
using AMMS.Hanet.Data.Response;
using MassTransit;

namespace AMMS.Hanet.Applications.V1.Consummer;

public class HANET_DEVICE_PUSHConsummer : IConsumer<Hanet_Device_Data>
{
    HANET_Device_Reponse_Service _HANET_Process_Data_Service;
    public HANET_DEVICE_PUSHConsummer(HANET_Device_Reponse_Service hANET_Process_Data_Service)
    {
        _HANET_Process_Data_Service = hANET_Process_Data_Service;
    }
    public async Task Consume(ConsumeContext<Hanet_Device_Data> context)
    {
        //Xử lý chèn vào CSDL nên sử dụng IMediator
        if (context == null || context.Message == null) return;

        await _HANET_Process_Data_Service.ProcessData(context.Message);
    }
}
