using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Server.Core.Identity.Entities;

public class ApplicationUser : IdentityUser
{
    public string? OrganizationId { get; set; } 
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    public bool OwnsToken(string token)
    {
        return RefreshTokens?.Find(x => x.Token == token) != null;
    }
    [MaxLength(50)]
    public string? PersonId { get; set; }
}