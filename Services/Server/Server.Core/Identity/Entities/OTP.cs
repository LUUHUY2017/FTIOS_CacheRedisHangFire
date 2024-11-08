using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Core.Identity.Entities;

[Table("OTP", Schema = "Identity")]
public class OTP
{
    [MaxLength(50)]
    public string Id { get; set; }
    [MaxLength(50)]
    public string? UserId { get; set; }
    [MaxLength(500)]
    public string Code { get; set; }

    [MaxLength(100)]
    public string Key { get; set; }

    [MaxLength(100)]
    public string OtpCode { get; set; }
    [MaxLength(50)]
    public string Type { get; set; }
    [MaxLength(1000)]
    public string Content { get; set; }
    public DateTime CreateTime { get; set; }
    public DateTime ExpTime { get; set; }
    public DateTime? VerifiedTime { get; set; }
    public bool? Verified { get; set; }

}
