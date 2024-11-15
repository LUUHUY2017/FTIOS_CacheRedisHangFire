using StackExchange.Redis;
using Newtonsoft.Json;
using Shared.Core.Loggers;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace Shared.Core.Caches.Redis;


public class CacheService : ICacheService
{
    private IDatabase _redis;
    private IConfiguration _configuration;
    private ConnectionMultiplexer connection;
    public CacheService(IConfiguration configuration)
    {
        _configuration = configuration;
        string url = $"{_configuration["Redis:Host"]}:{_configuration["Redis:Post"]}";
        connection = ConnectionMultiplexer.Connect(url);

        _redis = connection.GetDatabase();
    } 
    public async Task<T> GetData<T>(string key)
    {
        try
        {
            var value = await _redis.StringGetAsync(key);
            if (string.IsNullOrEmpty(value))
            {

            }
            else
            {
                var retVal = JsonConvert.DeserializeObject<T>(value);
                return retVal;
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
        return default(T);
    }
    public async Task<T> SetData<T>(string keyName, T value, DateTimeOffset expirationTime)
    {
        string json = "";
        try
        {
            TimeSpan expiryTime = expirationTime.DateTime.Subtract(DateTime.Now);
            json = JsonConvert.SerializeObject(value);
            //Logger.Info(keyName);
            //Logger.Info(json);
            await _redis.StringSetAsync(keyName, json, expiryTime);
            return value;
        }
        catch (Exception ex)
        {
            Logger.Error($"Error on keyName: {keyName}; Json: {json}");
            Logger.Error(ex);
        }
        return default;
    }
    public async Task<T> SetData<T>(string keyName, T value)
    {
        string json = "";
        try
        {
            json = JsonConvert.SerializeObject(value);
            //Logger.Info(keyName);
            //Logger.Info(json);
            await _redis.StringSetAsync(keyName, json);
            return value;
        }
        catch (Exception ex)
        {
            Logger.Error($"Error on keyName: {keyName}; Json: {json}");
            Logger.Error(ex);
        }
        return default;
    }

    public async Task<bool> RemoveData(string keyName)
    {
        try
        {
            bool _isKeyExist = await _redis.KeyExistsAsync(keyName);
            if (_isKeyExist == true)
            {
                var retVal = _redis.KeyDelete(keyName);
                return retVal;
            }
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
        }
        return false;
    }

    public async Task<bool> Removes(string pattern)
    {
        try
        {
            //ConnectionMultiplexer connection = ConnectionHelper.Connection;

           
            EndPoint endPoint = connection.GetEndPoints().First();
            RedisKey[] keys = connection.GetServer(endPoint).Keys(pattern: pattern).ToArray();

            var result = await _redis.KeyDeleteAsync(keys);
            return (result == keys.Length);
        }
        catch (Exception e)
        {
            Logger.Error(e);
            return false;
        }
    }

    public async Task<List<T>> Gets<T>(string pattern)
    {
        try
        {  
            EndPoint endPoint = connection.GetEndPoints().First();
            RedisKey[] keys = connection.GetServer(endPoint).Keys(pattern: pattern).ToArray();
            //var server = connection.GetServer(endPoint);

            var result = await _redis.StringGetAsync(keys);
            if (result != null && result.Length > 0)
            {
                List<T> lists = new List<T>();
                foreach (var item in result)
                {
                    var itemT = JsonConvert.DeserializeObject<T>(item);
                    lists.Add(itemT);
                }
                return lists;
            }
        }
        catch (Exception e)
        {
            Logger.Error(e);
            throw e;
        }
        return default;
    }
}
