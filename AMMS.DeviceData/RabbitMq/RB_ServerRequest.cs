using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AMMS.DeviceData.RabbitMq
{
    public class RB_ServerRequest
    {
        public string Id { get; set; }
        /// <summary>
        /// Loại sự kiện
        /// </summary>
        public string Action { get; set; }
        /// <summary>
        /// Đối tượng request
        /// </summary>
        public string RequestType { get; set; }
        /// <summary>
        /// Id thiết bị
        /// </summary>
        public string DeviceId { get; set; }
        /// <summary>
        /// Loại thiết bị
        /// </summary>
        public string DeviceModel { get; set; }
        /// <summary>
        /// ListData json
        /// </summary>
        public string? RequestParam { get; set; }

    }
    public class ServerRequestAction
    {
        public static string ActionAdd = "Add";
        public static string ActionDelete = "Delete";
        public static string ActionGetData = "GetData";
    }
    public class ServerRequestType
    {
        public static string UserInfo = "UserInfo";
        public static string UserFace = "UserFace";
        public static string UserFinger = "UserFinger";
    }
}
