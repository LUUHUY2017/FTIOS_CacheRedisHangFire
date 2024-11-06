using Shared.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMMS.VIETTEL.SMAS.Cores.Entities;

[Table("A0_Emailconfiguration")]
public class EmailConfiguration : EntityBase
{
    public string Server { get; set; }
    public string? UserName { get; set; }
    public string? PassWord { get; set; }
    public string? Email { get; set; }
    public int? Port { get; set; }
    public bool? EnableSSL { get; set; }
}
