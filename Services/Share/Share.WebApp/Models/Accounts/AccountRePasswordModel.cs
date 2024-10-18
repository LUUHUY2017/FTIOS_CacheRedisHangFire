using System.ComponentModel.DataAnnotations;

namespace AMMS.Share.WebApp.Models.Accounts;

public class AccountRePasswordModel : BaseViewModel
{
    [Required]
    public string Email { get; set; } = "";

    [Required]
    public string Password { get; set; } = "";

    [Required]
    public string ConfirmPassword { get; set; } = "";
}
