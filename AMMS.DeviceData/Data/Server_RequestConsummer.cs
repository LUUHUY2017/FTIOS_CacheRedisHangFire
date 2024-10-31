using AMMS.DeviceData.RabbitMq;
using EventBus.Messages;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMMS.DeviceData.Data
{
    public class Server_RequestConsummer : IConsumer<RB_ServerRequest>
    {
        private readonly IEventBusAdapter _eventBusAdapter;
        public Server_RequestConsummer(IEventBusAdapter eventBusAdapter)
        {
            _eventBusAdapter = eventBusAdapter;
        }

        public async Task Consume(ConsumeContext<RB_ServerRequest> context)
        {
            try
            {
                if (context.Message == null)
                    return;

                var data = context.Message;

                #region Zkteco
                if (data.DeviceModel.ToUpper() == "ZKTECO")
                {
                    var aa = await _eventBusAdapter.GetSendEndpointAsync(("ViettelZkteco") + EventBusConstants.ZK_Server_Push_S2D);
                    await aa.Send(data);
                }

                #endregion

                #region Hanet
                if (data.DeviceModel.ToUpper() == "HANET")
                {
                    var aa = await _eventBusAdapter.GetSendEndpointAsync(("ViettelHanet") + EventBusConstants.Hanet_Server_Push_S2D);
                    await aa.Send(data);
                }

                #endregion

                return;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
