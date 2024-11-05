using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMMS.DeviceData.RabbitMq
{
    public class TA_Device
    {
        public string Id { get; set; }
        /// <summary>
        /// Tên của máy muốn đẩy xuống
        /// </summary>
        public string DeviceName { get; set; }
        /// <summary>
        /// SerialNumber của máy muốn đẩy xuống
        /// </summary>
        public string SerialNumber { get; set; }
        /// <summary>
        /// SerialNumber của máy muốn đẩy xuống
        /// </summary>
        public string DeviceModel { get; set; }
        /// <summary>
        /// Ip của máy  
        /// </summary>
        public string? IpAdress { get; set; }
        /// <summary>
        /// Cổng của máy  
        /// </summary>
        public int? Port { get; set; }

    }
}
