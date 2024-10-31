using AMMS.DeviceData.RabbitMq;
using AMMS.Hanet.Applications.V1.Service;
using MassTransit;
using Shared.Core.Loggers;

namespace AMMS.Hanet.Applications.V1.Consummer
{
    public class HANET_SERVER_PUSHConsummer : IConsumer<RB_ServerRequest>
    {
        HANET_Server_Push_Service _hANET_Process_Service;
        public HANET_SERVER_PUSHConsummer(HANET_Server_Push_Service hANET_Process_Service)
        {
            _hANET_Process_Service = hANET_Process_Service;
        }

        public async Task Consume(ConsumeContext<RB_ServerRequest> context)
        {
            try
            {
                if (context == null || context.Message == null)
                    return;

                RB_ServerRequest data = context.Message;

                if (data == null || string.IsNullOrEmpty(data.Action) || string.IsNullOrEmpty(data.RequestType))
                    return;
                await _hANET_Process_Service.ProcessDataServerPush(data);
            }
            catch (Exception ex)
            {
                Logger.Warning(ex.Message);
            }
        }
    }
}
