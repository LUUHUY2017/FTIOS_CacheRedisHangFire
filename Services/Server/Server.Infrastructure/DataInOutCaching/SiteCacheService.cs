using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Server.Core.Entities;
using Shared.Core.Caches.Redis;
using Shared.Core.Loggers;

namespace Server.Infrastructure.DataInOutCaching;
public class SiteCacheService
{
    const string key = "Sites";
    const int timeExp_Minute = 10;

    private readonly BiDbContext _biDbContext;
    private readonly ICacheService _cacheService;
    private readonly IConfiguration _configuration;

    public SiteCacheService(BiDbContext biDbContext, ICacheService cacheService, IConfiguration configuration)
    {
        _biDbContext = biDbContext;
        _cacheService = cacheService;
        _configuration = configuration;
    }


    public async Task<List<Site>> LoadFromDb()
    {
        try
        {
            await Removes();

            var datas = await _biDbContext.Sites.ToListAsync();

            if (datas == null)
                datas = new List<Site>();
            if (datas.Count > 0)
            {
                foreach (var item in datas)
                {
                    if (await Get(item.Id) == null)
                        await Add(item);
                }
            }
            Logger.Information($"Load Sites from db: {datas.Count} rows");
            return datas;
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
        return new List<Site>();
    }
    public async Task<Site> Get(int siteId)
    {
        var entity = await _cacheService.GetData<Site>(GetKey(siteId));
        if (entity == null)
        {
            var obj = await _biDbContext.Sites.FirstOrDefaultAsync(o => o.Id == siteId);
            if (obj != null)
            {
                entity = obj;
                await Add(obj);
            }
        }
        return entity;
    }
    public async Task<Site> Add(Site data)
    {
        var sub_key = GetKey(data.Id);

        var cache = await _cacheService.GetData<Site>(sub_key);
        if (cache != null)
            await _cacheService.RemoveData(sub_key);

        var expirationTime = DateTime.Now.AddMinutes(timeExp_Minute);
        var entity = await _cacheService.SetData(sub_key, data, expirationTime);
        return entity;
    }

    public async Task<bool> Removes()
    {
        var sub_key = $"{_configuration.GetValue<string>("DataArea")}:{key}:*";
        var retVal = await _cacheService.Removes(sub_key);
        Logger.Information($"Clear cache {key}: {retVal}");
        return retVal;
    }

    private string GetKey(int siteId)
    {
        return $"{_configuration.GetValue<string>("DataArea")}:{key}:{siteId}";
    }
    public async Task<List<Site>> Gets()
    {
        var retVal = await _cacheService.Gets<Site>($"{_configuration.GetValue<string>("DataArea")}:{key}:*");
        if (retVal == null)
            retVal = await LoadFromDb();
        if (retVal == null)
            return new List<Site>();
        return retVal;
    }

}
