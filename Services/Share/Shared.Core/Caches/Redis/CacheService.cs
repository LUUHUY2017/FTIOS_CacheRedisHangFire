﻿using StackExchange.Redis;
using Newtonsoft.Json;
using Shared.Core.Loggers;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace Shared.Core.Caches.Redis;


public class CacheService : ICacheService
{
    private IDatabase _redis;
    public CacheService()
    {
        ConfigureRedis();
    }
    private void ConfigureRedis()
    {
        _redis = ConnectionHelper.Connection.GetDatabase();
    }
    public async Task<T> GetData<T>(string key)
    {
        try
        {
            var value = await _redis.StringGetAsync(key);
            if ( string.IsNullOrEmpty(value))
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
            ConnectionMultiplexer connection = ConnectionHelper.Connection;
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
            ConnectionMultiplexer connection = ConnectionHelper.Connection;
            //IDatabase db = connection.GetDatabase();
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

    public async Task<bool> Test()
    {
        try
        {
            ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("localhost");
            IDatabase db = redis.GetDatabase();

            //db.StringSet("foo", "bar");
            //Console.WriteLine(db.StringGet("foo")); // prints bar


            var hash = new HashEntry[] {
            new HashEntry("name", "John"),
            new HashEntry("surname", "Smith"),
            new HashEntry("company", "Redis"),
            new HashEntry("age", "29"),
        };
            db.HashSet("user-session:123", hash);


            var hashFields = db.HashGetAll("user-session:*");
            //Logger.Info(String.Join("; ", hashFields));

            return true;
        }
        catch(Exception e)
        {
            Logger.Error(e);
            return false;
        }
       
    }

}
