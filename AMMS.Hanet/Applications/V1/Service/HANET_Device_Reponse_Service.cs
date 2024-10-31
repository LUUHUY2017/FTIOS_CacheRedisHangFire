using AMMS.Hanet.Data.Response;
using AMMS.Hanet.Datas.Databases;

namespace AMMS.Hanet.Applications.V1.Service
{
    /// <summary>
    /// Xử lý data
    /// </summary>
    public class HANET_Device_Reponse_Service
    {
        DeviceAutoPushDbContext _deviceAutoPushDbContext;
        public HANET_Device_Reponse_Service(DeviceAutoPushDbContext deviceAutoPushDbContext)
        {
            _deviceAutoPushDbContext = deviceAutoPushDbContext;
        }
        /// <summary>
        /// Xử lý thông tin thiết bị trả về
        /// </summary>
        /// <returns></returns>
        public async Task ProcessData(Hanet_Device_Data reponse)
        {

        }
    }
}
