using Shared.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace Server.Core.Entities.A0
{
    public class A0_RoleGroupDetail : EntityBase
    {
        [MaxLength(50)]
        public string? RoleGroupId { get; set; }

        [MaxLength(50)]
        public string? RoleId { get; set; }
    }


}
