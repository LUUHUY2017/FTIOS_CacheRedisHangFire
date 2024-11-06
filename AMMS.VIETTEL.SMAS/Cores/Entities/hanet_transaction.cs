using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMMS.VIETTEL.SMAS.Cores.Entities;

[Table("transaction", Schema = "Hanet")]

public class hanet_transaction
{
    [Key]
    public string id { get; set; }
    public string content { get; set; }
    public string deviceID { get; set; }

}
