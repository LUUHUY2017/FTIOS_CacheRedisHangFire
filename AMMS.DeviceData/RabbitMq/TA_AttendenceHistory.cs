using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMMS.DeviceData.RabbitMq
{
    public class TA_AttendenceHistory
    {
        public string Id { get; set; }

        /// <summary>
        /// Id học sinh
        /// </summary>
        public string? PersonId { get; set; }
        /// <summary>
        /// Mã học sinh
        /// </summary>
        public string? PersonCode { get; set; }
        /// <summary>
        /// DeviceId
        /// </summary>
        public string? DeviceId { get; set; }
        public string? SerialNumber { get; set; }
        public DateTime? TimeEvent { get; set; }
        /// <summary>
        /// Loại sự kiện 
        /// </summary>
        public int? Type { get; set; }


    }
    public class TA_AttendenceImage
    {
        public string Id { get; set; }

        public string PersonId { get; set; }
        /// <summary>
        /// Mã học sinh
        /// </summary>
        public string? PersonCode { get; set; }
        public string? SerialNumber { get; set; }

        public DateTime? TimeEvent { get; set; }
        /// <summary>
        /// ImageBase64 ảnh điểm danh
        /// </summary>
        public string? ImageBase64 { get; set; }

    }
    /// <summary>
    /// Loại chấm công
    /// </summary>
    public enum TA_AttendenceType
    {
        None = 0,
        Card = 1,
        Face = 2,
        Finger = 3,
        Pass = 4,
    }
}
