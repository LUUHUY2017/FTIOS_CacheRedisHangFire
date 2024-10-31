using AMMS.ZkAutoPush.Applications.V1;
using AMMS.ZkAutoPush.Datas.Databases;
using AMMS.ZkAutoPush.Datas.Entities;
using Microsoft.EntityFrameworkCore;
using Shared.Core.Caches.Redis;
using Shared.Core.Loggers;

namespace AMMS.ZkAutoPush.Applications;

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
    public async Task<zk_terminal> Get(string serialNumber)
    {
        var entity = await _cacheService.GetData<zk_terminal>(GetKey(serialNumber));
        if (entity == null)
        {
            try
            {
                var obj = await _dbContext.zk_terminal.FirstOrDefaultAsync(o => o.sn == serialNumber);
                if (obj != null)
                {
                    entity = obj;
                    await Set(obj);
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }
        return entity;
    }
    public async Task<zk_terminal> Set(zk_terminal data)
    {
        var sub_key = GetKey(data.sn);

        var expirationTime = DateTime.Now.AddMinutes(timeExp_Minute);
        var entity = await _cacheService.SetData(sub_key, data, expirationTime);
        return entity;
    }
    private string GetKey(string serialNumber)
    {
        return $"{_configuration.GetValue<string>("DataArea")}:{key}:{serialNumber}";
    }
}

public class DeviceCommandCacheService
{
    const string key = "ZK_IclockCommand";
    const int timeExp_Minute = 10;

    private readonly DeviceAutoPushDbContext _dbContext;
    private readonly ICacheService _cacheService;
    private readonly IConfiguration _configuration;

    public DeviceCommandCacheService(DeviceAutoPushDbContext dbContext, ICacheService cacheService, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _cacheService = cacheService;
        _configuration = configuration;
    }

    /// <summary>
    /// Lấy tất cả các lệnh của thiết bị
    /// </summary>
    /// <param name="dataId"></param>
    /// <returns></returns>
    public async Task<List<IclockCommand>> Gets(string sn)
    {
        var entities = await _cacheService.Gets<IclockCommand>(GetKey(sn));
        return entities;
    }

    public async Task<IclockCommand> Get(string dataId)
    {
        var entity = await _cacheService.GetData<IclockCommand>(GetKey(dataId));
        if (entity == null)
        {
            try
            {
                var obj = await _dbContext.zk_terminalcommandlog.FirstOrDefaultAsync(o => o.terminal_sn == dataId);

                if (obj != null)
                {
                    //entity = obj;
                    //await Set(obj);
                }
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }
        return entity;
    }
    public async Task<bool> Clear(string sn)
    {
        var sub_key = GetKey(sn);
        var cache = await _cacheService.GetData<IclockCommand>(sub_key);
        if (cache != null)
            await _cacheService.RemoveData(sub_key);
        var retVal = await _cacheService.RemoveData(sub_key);

        return retVal;
    }















    //public async Task<IclockCommand> Get(string dataId)
    //{
    //    var entity = await _cacheService.GetData<IclockCommand>(GetKey(dataId));
    //    if (entity == null)
    //    {
    //        try
    //        {
    //            var obj = await _dbContext.zk_terminalcommandlog.FirstOrDefaultAsync(o => o.terminal_sn == dataId);

    //            if (obj != null)
    //            {
    //                //entity = obj;
    //                //await Set(obj);
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            Logger.Error(e);
    //        }
    //    }
    //    return entity;
    //}
    public async Task<zk_terminal> Set(zk_terminal data)
    {
        var sub_key = GetKey(data.sn);

        var expirationTime = DateTime.Now.AddMinutes(timeExp_Minute);
        var entity = await _cacheService.SetData(sub_key, data, expirationTime);
        return entity;
    }
    private string GetKey(string serialNumber)
    {
        return $"{_configuration.GetValue<string>("DataArea")}:{key}:{serialNumber}";
    }

    //private List<IclockCommand> Convert(List<zk_terminalcommandlog> cmds)
    //{

    //    return new List<IclockCommand>();
    //}
    //private IclockCommand Convert(zk_terminalcommandlog cmd)
    //{

    //    return new IclockCommand();
    //}
}
