using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMMS.ZkAutoPush.Datas.Entities
{
    [Table("zk_user", Schema = "Zkteco")]

    public class zk_user
    {
        [Key]
        public string Id { get; set; }
        public string? area_id { get; set; }
        public string? first_name { get; set; }
        public string? last_name { get; set; }
        public string? user_code { get; set; }
        public string? card_no { get; set; }
        public int? privilege { get; set;}
    }
}
