using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AMMS.ZkAutoPush.Datas.Entities;

[Table("zk_terminalcommandlog", Schema = "Zkteco")]
public class zk_terminalcommandlog
{
    [Key]
    public string Id { get; set; }
    public double command_id { get; set; }
    public string? content { get; set; }
    public string? terminal_id { get; set; }
    public DateTime? commit_time { get; set; }
    public DateTime? transfer_time { get; set; }
    public DateTime? return_time { get; set; }
    public int? return_value { get; set; }

}
