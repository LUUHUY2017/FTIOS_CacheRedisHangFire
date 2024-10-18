using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Server.Core.Entities;
using Server.Infrastructure.DataInOutCaching.Models;
using Shared.Core.Caches.Redis;
using Shared.Core.Loggers;

namespace Server.Infrastructure.DataInOutCaching;

public class PocDataInOutEventCacheRepo
{
    const string key = "PocDataInOutEvents";
    const int dayNum = 1;

    private readonly ICacheService _cacheService;
    private readonly BiDbContext _biDbContext;
    private readonly IConfiguration _configuration;
    public PocDataInOutEventCacheRepo(ICacheService cacheService, BiDbContext biDbContext, IConfiguration configuration)
    {
        _cacheService = cacheService;
        _biDbContext = biDbContext;
        _configuration = configuration;
    }
    public async Task<List<PocDataInOutEvent>> LoadFromDb()
    {
        try
        {
            //Xóa cache
            await Removes();

            var dayNum1 = dayNum * -1;
            var now = DateTime.Now.Date;
            DateTime lastDt = DateTime.Now.Date.AddDays(dayNum1);

            var datas = await _biDbContext.PocDataInOutEvents.Where(o => o.StartTime >= lastDt).ToListAsync();
            if (datas == null)
                datas = new List<PocDataInOutEvent>();
            if (datas.Count > 0)
            {
                foreach (var item in datas)
                {
                    if (!await Check(item))
                        await Add(item);
                }
            }
            //Logger.Debug($"Load PocDataInOutEvents from db: {datas.Count} rows");
            return datas;
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
            return null;
        }
    }

    public async Task<List<PocDataInOutEvent>> LoadFromDb(DateTime date)
    {
        try
        {  
            var datas = await _biDbContext.PocDataInOutEvents.Where(o => o.StartTime.Date == date).ToListAsync();
            if (datas == null)
                datas = new List<PocDataInOutEvent>();
            if (datas.Count > 0)
            {
                foreach (var item in datas)
                {
                    if (!await Check(item))
                        await Add(item);
                }
            }
            //Logger.Debug($"Load PocDataInOutEvents from db: {datas.Count} rows");
            return datas;
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
            return null;
        }
    }

    public async Task<bool> Check(PocDataInOutEvent data)
    {
        var entity = await _cacheService.GetData<PocDataInOutEvent>(GetKey(data.SerialNumber, data.StartTime));
        return entity != null;
    }
    public async Task<bool> Check(string serialNumner, DateTime startTime)
    {
        var entity = await _cacheService.GetData<PocDataInOutEvent>(GetKey(serialNumner, startTime));
        return entity != null;
    }
    public async Task<PocDataInOutEvent> Add(PocDataInOutEvent data)
    { 
        var expirationTime = DateTime.Now.AddDays(dayNum);

        var entity = await _cacheService.SetData(GetKey(data.SerialNumber, data.StartTime), data, expirationTime);
        return entity;
    }
    public async Task<PocDataInOutEvent> Update(PocDataInOutEvent data)
    {
        var sub_key = GetKey(data.SerialNumber, data.StartTime);

        var cache = await _cacheService.GetData<Terminal>(sub_key);
        if (cache != null)
            await _cacheService.RemoveData(sub_key);

        return await Add(data);
    }

    public async Task<bool> Removes()
    {
        var sub_key = $"{_configuration.GetValue<string>("DataArea")}:{key}:*";
        var retVal = await _cacheService.Removes(sub_key);
        //Logger.Debug($"Clear cache {key}: {retVal}");

        var sub_key1 = $"{_configuration.GetValue<string>("DataArea")}:Loaded{key}:*";
        var retVal1 = await _cacheService.Removes(sub_key1);

        return retVal;
    }
    private string GetKey(string serialNumber, DateTime startTime)
    {
        return $"{_configuration.GetValue<string>("DataArea")}:{key}:{serialNumber},{startTime.ToString("yyyyMMddHHmm")}";
    }

    public async Task<List<PocDataInOutEvent>> Gets()
    {
        var retVal = await _cacheService.Gets<PocDataInOutEvent>($"{_configuration.GetValue<string>("DataArea")}:{key}:*");
        //if (retVal == null)
        //    retVal = await LoadFromDb();
        if (retVal == null)
            return new List<PocDataInOutEvent>();
        return retVal;
    }

    // <summary>
    /// Lấy danh sách toàn bộ dữ liệu từ bộ đệm có filter
    /// </summary>
    /// <returns></returns>
    public async Task<List<PocDataInOutEvent>> Gets(DataInOutLogFilter filter)
    {
        var cacheData = await _cacheService.Gets<PocDataInOutEvent>($"{_configuration.GetValue<string>("DataArea")}:{key}:*");
        //var cacheData = _cacheService.GetData<List<PocDataInOutEvent>>(key);
        var startTime = filter.ConvertStartTime();
        if (cacheData == null)
            cacheData = new List<PocDataInOutEvent>();
        cacheData = cacheData.Where(x => ((!string.IsNullOrEmpty(filter.Key) && filter.ColumnTable == "serial_number") ? (x.SerialNumber.ToLower().Contains(filter.Key.ToLower().Trim())) : true)
                                        && ((filter.OrganizationId != -1) ? (filter.OrganizationId == x.OrganizationId) : true)
                                        && ((filter.SiteId != -1) ? (filter.SiteId == x.SiteId) : true)
                                        && ((filter.LocationId != -1) ? (filter.LocationId == x.LocationId) : true)
                                        && (!string.IsNullOrEmpty(filter.StartTime) ? (((DateTime)startTime).Date == x.StartTime.Date) : true)
                                )
                            .OrderByDescending(x => x.StartTime)
                            .ToList();
        return cacheData;
    }


    public async Task<bool> CheckToCacheByDay(DateTime date)
    {
        var sub_key = $"{_configuration.GetValue<string>("DataArea")}:Loaded{key}:{date.ToString("yyyyMMdd")}";
        var entity = await _cacheService.GetData<bool>(sub_key);
        return entity;
    }
    public async Task<bool> SetToCacheByDay(DateTime date)
    {
        try
        {
            var sub_key = $"{_configuration.GetValue<string>("DataArea")}:Loaded{key}:{date.ToString("yyyyMMdd")}";
            var expirationTime = DateTime.Now.AddDays(dayNum);

            var retVal = await _cacheService.SetData<bool>(sub_key, true, expirationTime);

            var datas = await _biDbContext.PocDataInOutEvents.Where(o => o.StartTime.Date == date.Date).ToListAsync();
            if (datas == null)
                datas = new List<PocDataInOutEvent>();
            if (datas.Count > 0)
            {
                foreach (var item in datas)
                {
                    if (!await Check(item))
                        await Add(item);
                }
            }


            //Logger.Debug($"Load PocDataInOutEvents from db  by day {date.ToString("yyyyMMdd")}: {datas.Count} rows");

            return retVal;
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
            return false;
        }
    }

}

