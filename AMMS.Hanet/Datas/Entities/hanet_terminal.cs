using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMMS.Hanet.Datas.Entities
{
    [Table("terminal", Schema = "Hanet")]

    public class hanet_terminal
    {
        [Key]
        public string Id { get; set; }
        public string? area_id { get; set; }
        public DateTime? create_time { get; set; }
        public DateTime? change_time { get; set; }
        public string? create_user { get; set; }
        public string? change_user { get; set; }
        public string sn { get; set; }
        public string name { get; set; }
        public string? ip_address { get; set; }
        public int? port { get; set; }
        public int? user_count { get; set; }
        public int? face_count { get; set; }
        public int? fv_count { get; set; }
        public DateTime? last_activity { get; set; }
        public DateTime? upload_time { get; set; }
        public DateTime? push_time { get; set; }
        public DateTime? last_checkconnection { get; set; }
        public DateTime? time_offline { get; set; }
        public DateTime? time_online { get; set; }
        public bool? online_status { get; set; }

    }
}
