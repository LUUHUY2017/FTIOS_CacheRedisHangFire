using AMMS.DeviceData.RabbitMq;
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
        serialNumber = serialNumber.ToUpper();

        var entity = await _cacheService.GetData<zk_terminal>(GetKey(serialNumber));
        if (entity == null)
        {
            try
            {
                var obj = await _dbContext.zk_terminal.FirstOrDefaultAsync(o => o.sn == serialNumber);
                if (obj != null)
                {
                    entity = obj;
                }
                else
                {
                    obj = new zk_terminal();
                    obj.sn = serialNumber;
                    obj.name = serialNumber;
                    obj.Id = Guid.NewGuid().ToString();
                    obj.create_time = DateTime.Now;
                    _dbContext.zk_terminal.Add(obj);
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
    public async Task<zk_terminal> Set(zk_terminal data)
    {
        var sub_key = GetKey(data.sn);

        var expirationTime = DateTime.Now.AddMinutes(timeExp_Minute);
        var entity = await _cacheService.SetData(sub_key, data, expirationTime);
        return entity;
    }
    public async Task<bool> Remove(string serialNumber)
    {
        var sub_key = GetKey(serialNumber);
        await _cacheService.Removes(sub_key);
        return true;
    }
    public async Task<List<zk_terminal>> Gets()
    {

        var listterminal = await _cacheService.Gets<zk_terminal>(GetKey());

        return listterminal;
    }

    private string GetKey(string serialNumber)
    {
        return $"{_configuration.GetValue<string>("DataArea")}_{EventBusConstants.ZKTECO}:{key}:{serialNumber}";
    }
    private string GetKey()
    {
        return $"{_configuration.GetValue<string>("DataArea")}_{EventBusConstants.ZKTECO}:{key}:*";
    }

    //Cập nhật thông tin vào caches
    public async Task<bool> Save(zk_terminal data)
    {
        var sub_key = GetKey(data.sn);
        var entity = await _cacheService.GetData<zk_terminal>(sub_key);
        if (entity != null)
        {
            await _cacheService.RemoveData(sub_key);
        }
        var expirationTime = DateTime.Now.AddMinutes(timeExp_Minute);
        await _cacheService.SetData(sub_key, data, expirationTime);

        return true;
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
    public async Task<IclockCommand> GetByCode(string sn, string id)
    {
        var entity = await _cacheService.GetData<IclockCommand>(GetKey(sn, id));

        return entity;
    }

    public async Task<bool> Remove(string sn, string id)
    {
        var sub_key = GetKey(sn, id);
        await _cacheService.RemoveData(sub_key);
        return true;
    }
    public async Task<bool> RemoveAll(string sn)
    {
        var listterminal = await _cacheService.Gets<IclockCommand>(GetKey(sn));
        foreach (var terminal in listterminal)
        {
            var sub_key = GetKey(sn, terminal.Id.ToString());
            await _cacheService.RemoveData(sub_key);
        }

        return true;
    }

    /// <summary>
    /// Set lệnh vào
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public async Task<IclockCommand> Set(IclockCommand data)
    {
        //var expirationTime = DateTime.Now.AddMinutes(timeExp_Minute);
        var sub_key = GetKey(data.SerialNumber, data.Id.ToString());
        var entity = await _cacheService.SetData(sub_key, data);
        return entity;
    }

    //Cập nhật thông tin vào caches
    public async Task<bool> Save(IclockCommand data)
    {
        var sub_key = GetKey(data.SerialNumber, data.Id.ToString());
        var entity = await _cacheService.GetData<IclockCommand>(sub_key);
        if (entity != null)
        {
            await _cacheService.RemoveData(sub_key);
        }
        var expirationTime = DateTime.Now.AddMinutes(timeExp_Minute);
        await _cacheService.SetData(sub_key, data, expirationTime);

        return true;
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
    private string GetKey(string serialNumber, string id)
    {
        return $"{_configuration.GetValue<string>("DataArea")}_{EventBusConstants.ZKTECO}:{key}:{serialNumber}:{id} ";
    }

    public async Task<List<IclockCommand>> Gets(string sn)
    {

        var listterminal = await _cacheService.Gets<IclockCommand>(GetKey(sn));

        return listterminal;
    }
    private string GetKey(string serialNumber)
    {
        return $"{_configuration.GetValue<string>("DataArea")}_{EventBusConstants.ZKTECO}:{key}:{serialNumber}:*";
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
