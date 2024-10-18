using Shared.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace Server.Core.Entities.A0
{

    public class A0_RoleGroupPage : EntityBase
    {
        [MaxLength(50)]
        public string? PageId { get; set; }

        [MaxLength(50)]
        public string? RoleGroupId { get; set; }

        [MaxLength(250)]
        public string? Description { get; set; }
    }


}
