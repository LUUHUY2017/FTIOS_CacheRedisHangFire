using System.ComponentModel.DataAnnotations;

namespace AMMS.Share.WebApp.Models.Accounts;

public class ForgotPasswordModel
{
    [Required]
    public string Email { get; set; } = "";
}
