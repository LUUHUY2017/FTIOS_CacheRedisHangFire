using Shared.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Core.Entities.A0;

[Table("Page")]
public class A0_Page : EntityBase
{
    [MaxLength(50)]
    public string? Module { get; set; }
    [MaxLength(250)]
    public string? RolePermission { get; set; }
    [MaxLength(250)]
    public string? Url { get; set; }
    [MaxLength(250)]
    public string? Name { get; set; }
    [MaxLength(250)]
    public string? Description { get; set; }
}
