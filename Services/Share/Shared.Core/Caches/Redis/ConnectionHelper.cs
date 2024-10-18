using StackExchange.Redis;


namespace Shared.Core.Caches.Redis;

public class ConnectionHelper
{
    static ConnectionHelper()
    {
        ConnectionHelper.lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            //"127.0.0.1:6379"
            //return ConnectionMultiplexer.Connect("127.0.0.1:6379");
            return ConnectionMultiplexer.Connect(ConfigurationManager.AppSetting["RedisURL"]);
        });
    }

    private static Lazy<ConnectionMultiplexer> lazyConnection;

    public static ConnectionMultiplexer Connection
    {
        get
        {
            return lazyConnection.Value;
        }
    }
}
