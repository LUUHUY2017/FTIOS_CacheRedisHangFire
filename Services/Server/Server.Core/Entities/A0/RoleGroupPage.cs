using Shared.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Core.Entities.A0;

[Table("RoleGroupPage")]

    public class RoleGroupPage : EntityBase
    {
        [MaxLength(50)]
        public string? PageId { get; set; }

    [MaxLength(50)]
    public string? RoleGroupId { get; set; }

    [MaxLength(250)]
    public string? Description { get; set; }
}
