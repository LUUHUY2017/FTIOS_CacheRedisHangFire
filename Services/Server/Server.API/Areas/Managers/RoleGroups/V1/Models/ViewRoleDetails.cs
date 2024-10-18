using Microsoft.AspNetCore.Identity;
using Server.Core.Identity.Entities;

namespace AMMS.WebAPI.Areas.Manage.RoleGroups.V1.Models
{
    public class ViewRoleDetails
    {
        public List<IdentityRole> DatasSelected { get; set; }
        public List<IdentityRole> DatasUnSelected { get; set; }
    }

    public class ViewRoleGroupUser
    {
        public List<ApplicationUser> DatasSelected { get; set; }
        public List<ApplicationUser> DatasUnSelected { get; set; }
    }
}
