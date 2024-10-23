using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMMS.ZkAutoPush.Datas.Entities
{
    [Table("zk_biophoto", Schema = "Zkteco")]

    public class zk_biophoto
    {
        [Key]
        public string Id { get; set; }
        public string? PersonId { get; set; }
        public string? Folder { get; set; }
        public string? FileName { get; set; }

    }
}
