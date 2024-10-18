using System.ComponentModel.DataAnnotations;

namespace Server.Core.Identity.Interfaces.Accounts.Requests;

public class ForgotPasswordRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
