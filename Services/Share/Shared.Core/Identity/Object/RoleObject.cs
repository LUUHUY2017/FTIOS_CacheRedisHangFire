using Microsoft.AspNetCore.Identity;

namespace Shared.Core.Identity.Object;

public class RoleObject
{
    public string Id { get; set; }
    public string Module { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public IdentityRole ToIdentityRole()
    {
        return new IdentityRole()
        {
            Id = Id,
            Name = Name,
            NormalizedName = Description,
        };
    }
}
