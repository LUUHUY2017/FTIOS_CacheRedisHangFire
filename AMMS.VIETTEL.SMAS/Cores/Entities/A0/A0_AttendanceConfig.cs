using Shared.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMMS.VIETTEL.SMAS.Cores.Entities.A0;

[Table("A0_Attendanceconfig")]
public class A0_AttendanceConfig : EntityBase
{
    public string? EndpointIdentity { get; set; }
    public string? AccountName { get; set; }
    public string? Password { get; set; }
    public string? GrantType { get; set; }
    public string? Scope { get; set; }
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }

    public static string key = "r0QQKLBa3x9KN/8el8Q/HQ==";
    public static string keyIV = "8bCNmt1+RHBNkXRx8MlKDA==";
    public static string secretKey = "SMas$#3/*/lsn_diem_danh";

    public string? EndpointGateway { get; set; }
    public DateTime? TimeAsync { get; set; }

    public string? Note { get; set; }

}