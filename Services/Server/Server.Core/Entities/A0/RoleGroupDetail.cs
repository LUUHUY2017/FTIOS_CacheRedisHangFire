using Shared.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Core.Entities.A0
{
    public class RoleGroupDetail : EntityBase
    {
        [MaxLength(50)]
        public string? RoleGroupId { get; set; }

    [MaxLength(50)]
    public string? RoleId { get; set; }
}
