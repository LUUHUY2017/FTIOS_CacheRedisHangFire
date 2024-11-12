using StackExchange.Redis;


namespace Shared.Core.Caches.Redis;

public class ConnectionHelper
{
    static ConnectionHelper()
    {
        ConnectionHelper.lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        {
            return ConnectionMultiplexer.Connect("127.0.0.1:6379");
            //string url = $"{ConfigurationManager.AppSetting["Redis:Host"]}:{ConfigurationManager.AppSetting["Redis:Post"]}";
            //return ConnectionMultiplexer.Connect(url);
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
