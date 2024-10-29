using AMMS.ZkAutoPush.Datas.Databases;
using AMMS.ZkAutoPush.Datas.Entities;

namespace AMMS.ZkAutoPush.Applications.V1
{
    public class StartupDataService
    {
        private readonly DeviceAutoPushDbContext _deviceAutoPushDbContext;

        public StartupDataService(DeviceAutoPushDbContext deviceAutoPushDbContext)
        {
            _deviceAutoPushDbContext = deviceAutoPushDbContext;
        }

        public async Task<zk_terminal> GetDevice(string sn)
        {
            sn = sn.ToUpper();
            var thietbi = ZK_SV_PUSHService.ListTerminal.FirstOrDefault(x => x.sn == sn);
            if (thietbi == null)
            {
                thietbi = _deviceAutoPushDbContext.zk_terminal.FirstOrDefault(x => x.sn == sn);
                if(thietbi == null)
                {
                    thietbi = new zk_terminal();
                    thietbi.sn = sn;
                    thietbi.Id = Guid.NewGuid().ToString();
                    thietbi.create_time = DateTime.Now;
                    _deviceAutoPushDbContext.zk_terminal.Add(thietbi);
                    _deviceAutoPushDbContext.SaveChanges();
                }
                ZK_SV_PUSHService.ListTerminal.Add(thietbi);
            }
            return thietbi;
        }
    }
}
