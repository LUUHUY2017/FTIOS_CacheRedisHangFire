using Shared.Core.Loggers;
using StackExchange.Redis;
using System;

public interface IRedisNotificationService
{
    void SubscribeToKeyspaceEvents();
}

public class RedisNotificationService : IRedisNotificationService
{
    private readonly ISubscriber _subscriber;

    public RedisNotificationService(IConnectionMultiplexer connectionMultiplexer)
    {
        _subscriber = connectionMultiplexer.GetSubscriber();
    }

    public void SubscribeToKeyspaceEvents()
    {
        _subscriber.Subscribe("__keyevent@0__:expired", (channel, value) =>
        //_subscriber.Subscribe("ordersystem", (channel, value) =>
        //_subscriber.Subscribe("__keyspace@0__:mykey del", (channel, value) =>
        //_subscriber.Subscribe("__keyevent@0__:del mykey", (channel, value) =>
        {
            Console.WriteLine($"Key expired: channel={channel} => value={value}");
            //Logger.ShowLog($"Key expired: channel={channel} => value={value}");
        });

        Console.WriteLine("Listening for keyspace events...");
        //Logger.ShowLog("Listening for keyspace events...");
    }
}