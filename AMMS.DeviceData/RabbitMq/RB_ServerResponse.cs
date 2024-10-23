using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMMS.DeviceData.RabbitMq
{
    public class RB_ServerResponse
    {
        public string Id { get; set; }
        public string? RequestId { get; set; }
        public string? ReponseType { get; set; }
        public string? Action { get; set; }
        public string? Content { get; set; }

    }
}
