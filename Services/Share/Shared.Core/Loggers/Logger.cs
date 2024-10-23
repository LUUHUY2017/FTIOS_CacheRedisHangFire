
namespace Shared.Core.Loggers;

public class Logger
{
    public static void Warning(string message)
    {
        Console.WriteLine(message);
        Serilog.Log.Warning(message);
    }
    public static void Information(string message)
    {
        Serilog.Log.Information(message);
    }
    public static void Error(string message)
    {
        Console.WriteLine(message);
        Serilog.Log.Error(message);
    }

    public static void Error(Exception ex)
    {
        Console.WriteLine(ex.Message);

        Serilog.Log.Error(ex.Message);
        Serilog.Log.Error(ex.StackTrace);
    }
}

