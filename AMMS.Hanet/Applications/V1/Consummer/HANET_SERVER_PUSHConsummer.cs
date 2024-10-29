using AMMS.DeviceData.RabbitMq;
using MassTransit;
using Shared.Core.Loggers;

namespace AMMS.Hanet.Applications.V1.Consummer
{
    public class HANET_SERVER_PUSHConsummer : IConsumer<RB_ServerRequest>
    {

        public HANET_SERVER_PUSHConsummer()
        {
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

            }
            catch (Exception ex)
            {
                Logger.Warning(ex.Message);
            }
        }
    }
}
