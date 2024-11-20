namespace AMMS.ZkAutoPush.Applications.TerminalCommandLogs.V1.Models;

public class TerminalCommandLogFilter
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public string? ColumnTable { get; set; }
    public string? Key { get; set; }
    public string? Status { get; set; }
}
