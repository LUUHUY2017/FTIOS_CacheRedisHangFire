using System.ComponentModel.DataAnnotations;

namespace AMMS.VIETTEL.SMAS.Cores.Identity.Interfaces.Accounts.Requests;

public class ForgotPasswordRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
