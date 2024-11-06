using AMMS.DeviceData.RabbitMq;
using AMMS.Hanet.Datas.Databases;
using AMMS.Hanet.Datas.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Core.Caches.Redis;
using Shared.Core.Loggers;

namespace AMMS.Hanet.Applications
{
    public class DeviceCacheService
    {
        const string key = "Terminals";
        const int timeExp_Minute = 10;

        private readonly DeviceAutoPushDbContext _dbContext;
        private readonly ICacheService _cacheService;
        private readonly IConfiguration _configuration;

        public DeviceCacheService(DeviceAutoPushDbContext dbContext, ICacheService cacheService, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _cacheService = cacheService;
            _configuration = configuration;
        }
        public async Task<List<hanet_terminal>> Gets()
        {

            var listterminal = await _cacheService.Gets<hanet_terminal>(GetKey());

            return listterminal;
        }
        public async Task<hanet_terminal> Get(string serialNumber)
        {
            serialNumber = serialNumber.ToUpper();

            var entity = await _cacheService.GetData<hanet_terminal>(GetKey(serialNumber));
            if (entity == null)
            {
                try
                {
                    var obj = await _dbContext.hanet_terminal.FirstOrDefaultAsync(o => o.sn == serialNumber);
                    if (obj != null)
                    {
                        entity = obj;
                    }
                    else
                    {
                        obj = new hanet_terminal();
                        obj.sn = serialNumber;
                        obj.Id = Guid.NewGuid().ToString();
                        obj.create_time = DateTime.Now;
                        _dbContext.hanet_terminal.Add(obj);
                        _dbContext.SaveChanges();
                    }
                    await Set(obj);

                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
            return entity;
        }
        public async Task<hanet_terminal> Set(hanet_terminal data)
        {
            var sub_key = GetKey(data.sn);

            var expirationTime = DateTime.Now.AddMinutes(timeExp_Minute);
            //var expirationTime = -1;
            var entity = await _cacheService.SetData(sub_key, data, expirationTime);
            return entity;
        }
        public async Task<bool> Remove(string serialNumber)
        {
            var sub_key = GetKey(serialNumber);
            await _cacheService.Removes(sub_key);
            return true;
        }
        //Cập nhật thông tin vào caches
        public async Task<bool> Save(hanet_terminal data)
        {
            var sub_key = GetKey(data.sn);
            var entity = await _cacheService.GetData<hanet_terminal>(sub_key);
            if (entity != null)
            {
                await _cacheService.RemoveData(sub_key);
            }
            var expirationTime = DateTime.Now.AddMinutes(timeExp_Minute);
            await _cacheService.SetData(sub_key, data, expirationTime);

            return true;
        }
        private string GetKey(string serialNumber)
        {
            return $"{_configuration.GetValue<string>("DataArea")}_{EventBusConstants.HANET}:{key}:{serialNumber}";
        }
        private string GetKey()
        {
            return $"{_configuration.GetValue<string>("DataArea")}_{EventBusConstants.HANET}:{key}:*";
        }
    }

}
