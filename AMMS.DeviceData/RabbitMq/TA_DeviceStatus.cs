using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMMS.DeviceData.RabbitMq
{
    public class TA_DeviceStatus
    {
        public string Id { get; set; }
        public string DeviceId { get; set; }

        /// <summary>
        /// SerialNumber của máy muốn đẩy xuống
        /// </summary>
        public string SerialNumber { get; set; }
        /// <summary>
        /// Trạng thái kết nối true/ kết nối. false/ chưa kết nôi
        /// </summary>
        public bool ConnectStatus { get; set; }
        /// <summary>
        /// Số user
        /// </summary>
        public int? UserCount { get; set; }
        /// <summary>
        /// Số khuôn mặt
        /// </summary>
        public int? FaceCount { get; set; }
        /// <summary>
        /// Số vân tay
        /// </summary>
        public int? FingerCount { get; set; }
        public DateTime? LastUpdate { get; set; }

    }
}
