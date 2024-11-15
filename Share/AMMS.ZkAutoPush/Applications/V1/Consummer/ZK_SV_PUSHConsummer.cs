using AMMS.DeviceData.RabbitMq;
using AMMS.ZkAutoPush.Data;
using MassTransit;
using Shared.Core.Loggers;

namespace AMMS.ZkAutoPush.Applications.V1.Consummer
{
    public class ZK_SV_PUSHConsummer : IConsumer<RB_ServerRequest>
    {

        ZK_SV_PUSHService _ZK_SV_PUSHService;
        public ZK_SV_PUSHConsummer(ZK_SV_PUSHService zK_SV_PUSHService)
        {
            _ZK_SV_PUSHService = zK_SV_PUSHService;
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

                 await _ZK_SV_PUSHService.Process(data);
            }
            catch (Exception ex)
            {
                Logger.Warning(ex.Message);
            }
        }
    }
}
