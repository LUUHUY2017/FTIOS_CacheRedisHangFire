using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMMS.ZkAutoPush.Datas.Entities;

[Table("zk_transaction", Schema = "Zkteco")]
public class zk_transaction
{
    [Key]
    public string Id { get; set; }
    public string? PersonId { get; set; }
    public string? PersonCode { get; set; }
    public string? IpAddress { get; set; }
    public string? DeviceId { get; set; }
    public string? SerrialNumber { get; set; }
    public string? Content { get; set; }

    public int? VerifyType { get; set; }
    public DateTime? TimeEvent { get; set; }
    public DateTime? CreatedTime { get; set; }

}
