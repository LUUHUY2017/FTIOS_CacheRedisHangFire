using AMMS.Hanet.Datas.Databases;

namespace AMMS.Hanet.Applications.V1.Service
{
    public class HANET_Process_Service
    {
        DeviceAutoPushDbContext _deviceAutoPushDbContext;
        public HANET_Process_Service(DeviceAutoPushDbContext deviceAutoPushDbContext)
        {
            _deviceAutoPushDbContext = deviceAutoPushDbContext;
        }



    }
}
