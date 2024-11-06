using Shared.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMMS.VIETTEL.SMAS.Cores.Entities;

[Table("app_config", Schema = "Hanet")]

[Table("A0_Attendanceconfig")]
public class app_config : EntityBase
{
    public string? EndpointIdentity { get; set; }
    public string? AccountName { get; set; }
    public string? Password { get; set; }
    public string? GrantType { get; set; }
    public string? Scope { get; set; }
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }

    public string? EndpointGateway { get; set; }
    public DateTime? TimeAsync { get; set; }

    public string? Note { get; set; }

}