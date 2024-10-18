
namespace Shared.Core.Loggers;

public class Logger
{
    public static void Warning(string message)
    {
        Serilog.Log.Warning(message);
    }
    public static void Information(string message)
    {
        Serilog.Log.Information(message);
    }
    public static void Error(string message)
    {
        Serilog.Log.Error(message);
    }

    public static void Error(Exception ex)
    {
        Serilog.Log.Error(ex.Message);
        Serilog.Log.Error(ex.StackTrace);
    }
}

