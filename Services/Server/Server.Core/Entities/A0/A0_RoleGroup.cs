using Shared.Core.Entities;

namespace Server.Core.Entities.A0
{
    public class A0_RoleGroup : EntityBase
    {
        public string? Name { get; set; }
        public string? Descriptions { get; set; }

        public virtual List<A0_RoleGroupDetail> RoleGroupDetails { get; set; }
        public virtual List<A0_RoleGroupUser> RoleGroupUsers { get; set; }

    }


}
