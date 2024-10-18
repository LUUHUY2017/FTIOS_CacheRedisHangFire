using Shared.Core.Loggers;

namespace Share.WebApp.CountHttpRequests.V1;

public class CountHttpRequest
{
    static DateTime DateTimeMinute { get; set; }
    static int CountValue { get; set; } = -1;

    public static void Count()
    {
        if (CountValue == -1)
        {
            CountValue = 1;
            DateTimeMinute = DateTime.Now;
        }
        else
        {
            CountValue++;
            DateTime curent = DateTime.Now;
            if ((DateTime.Now - DateTimeMinute).TotalSeconds > 60)
            {
                Logger.Warning($"Số request từ {DateTimeMinute.ToString("yyyyMMdd HH:mm:ss.fff")} đến {curent.ToString("yyyyMMdd HH:mm:ss.fff")} là {CountValue}  reqs");
                DateTimeMinute = curent;
                CountValue = 0;
            }

        }
    }
}
