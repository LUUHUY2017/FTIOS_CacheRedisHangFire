using Shared.Core.Entities;

namespace Server.Core.Entities.A0
{
    public class RoleGroup : EntityBase
    {
        public string? Name { get; set; }
        public string? Descriptions { get; set; }

        public virtual List<RoleGroupDetail> RoleGroupDetails { get; set; }
        public virtual List<RoleGroupUser> RoleGroupUsers { get; set; }

    }


}
