using Microsoft.Extensions.Logging;

namespace Shared.Core.Loggers;

public interface IMyService
{
    void LogWarning(string message);
    void LogError(string message);
    void ShowLog(Exception e);
}

public class MyService : IMyService
{
    private ILogger<MyService> _logger;

    public MyService(ILogger<MyService> logger)
    {
        _logger = logger;
    }
    public void LogWarning(string message)
    {
        _logger.LogWarning(message);
    }

    public void LogError(string message)
    {
        _logger.LogError(message);
    }
    public void ShowLog(Exception e)
    {
        _logger.LogError(e.Message);
        _logger.LogError(e.StackTrace);
    }

}