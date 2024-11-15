using AMMS.DeviceData.RabbitMq;
using EventBus.Messages;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Shared.Core.Loggers;

namespace AMMS.DeviceData.Data
{
    public class Server_RequestConsummer : IConsumer<RB_ServerRequest>
    {
        private readonly IEventBusAdapter _eventBusAdapter;
        private readonly IConfiguration _configuration;

        public Server_RequestConsummer(IEventBusAdapter eventBusAdapter, IConfiguration configuration)
        {
            _eventBusAdapter = eventBusAdapter;
            _configuration = configuration;
        }

        public async Task Consume(ConsumeContext<RB_ServerRequest> context)
        {
            try
            {
                if (context.Message == null)
                    return;

                var data = context.Message;

                if (data == null) return;


                #region Zkteco
                if (data.DeviceModel.ToUpper() == EventBusConstants.ZKTECO)
                {
                    var aa = await _eventBusAdapter.GetSendEndpointAsync($"{_configuration.GetValue<string>("DataArea")}_{EventBusConstants.ZKTECO}{EventBusConstants.ZK_Server_Push_S2D}");
                    await aa.Send(data);
                }

                #endregion

                #region Hanet
                if (data.DeviceModel.ToUpper() == EventBusConstants.HANET)
                {
                    var aa = await _eventBusAdapter.GetSendEndpointAsync($"{_configuration.GetValue<string>("DataArea")}_{EventBusConstants.HANET}{EventBusConstants.Hanet_Server_Push_S2D}");
                    await aa.Send(data);
                }

                #endregion

                return;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
}
