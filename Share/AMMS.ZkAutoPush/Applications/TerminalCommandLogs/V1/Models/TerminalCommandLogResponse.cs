namespace AMMS.ZkAutoPush.Applications.TerminalCommandLogs.V1.Models;

public class TerminalCommandLogResponse
{
    public string Id { get; set; }
    public DateTime? create_time { get; set; }
    public DateTime? change_time { get; set; }
    public string? return_content { get; set; }
    public string? terminal_id { get; set; }
    public string? terminal_sn { get; set; }
    public DateTime? commit_time { get; set; }
    public DateTime? transfer_time { get; set; }
    public DateTime? return_time { get; set; }
    public int? return_value { get; set; }
    public bool? successed { get; set; }
    public string? command_type { get; set; }
    public string? command_ation { get; set; }
}
