using AMMS.DeviceData.RabbitMq;
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
        public Task Consume(ConsumeContext<RB_ServerRequest> context)
        {
            throw new NotImplementedException();
        }
    }
}
