using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMMS.Hanet.Datas.Entities
{
    [Table("hanet_transaction", Schema = "Hanet")]

    public class hanet_transaction
    {
        [Key]
        public string id { get; set; }
        public string content { get; set; }
        public string deviceID { get; set; }
        /// <summary>
        ///  Timestamp tại thời điểm camera checkin.
        /// </summary>
        public double? time { get; set; }

        public string? transaction_type { get; set; }
        public DateTime? created_time { get; set; }

    }
}
