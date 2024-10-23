using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMMS.ZkAutoPush.Datas.Entities;

[Table("zk_biodata", Schema = "Zkteco")]
public class zk_biodata
{
    [Key]
    public string Id { get; set; }
    public string? PersonId { get; set; }
    /// <summary>
    /// Vị trí vân tay
    /// </summary>
    public int? FingerIndex { get; set; }
    /// <summary>
    /// Dữ liệu vân tay
    /// </summary>
    public string? FingerData { get; set; }

}
