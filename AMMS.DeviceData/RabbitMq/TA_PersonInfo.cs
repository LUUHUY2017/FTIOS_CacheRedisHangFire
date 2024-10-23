using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMMS.DeviceData.RabbitMq
{
    public class TA_PersonInfo
    {
        public string Id { get; set; }
        /// <summary>
        /// Mã người
        /// </summary>
        public string PersonCode { get; set; }

        public string? FisrtName { get; set; }
        public string? LastName { get; set; }
        /// <summary>
        /// ImageBase64 ảnh nhận dạng
        /// </summary>
        public string? UserFace { get; set; }
        public string? UserCard { get; set; }
        public string DeviceId { get; set; }

        /// <summary>
        /// SerialNumber của máy muốn đẩy xuống
        /// </summary>
        public string SerialNumber { get; set; }
        /// <summary>
        /// Model của máy muốn đẩy xuống //Zkteco, Hanet
        /// </summary>
        public string DeviceModel { get; set; }
    }
    public class TA_PersonFace
    {
        public string Id { get; set; }
        /// <summary>
        /// Mã học sinh
        /// </summary>
        public string PersonCode { get; set; }
        /// <summary>
        /// ImageBase64 ảnh nhận dạng
        /// </summary>
        public string? UserFace { get; set; }
 
        /// <summary>
        /// SerialNumber của máy muốn đẩy xuống
        /// </summary>
        public string SerialNumber { get; set; }
        /// <summary>
        /// Model của máy muốn đẩy xuống //Zkteco, Hanet
        /// </summary>
        public string DeviceModel { get; set; }
    }

}
