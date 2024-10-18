using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace AMMS.Share.WebApp.Models.Accounts;

public class RegisterModel
{
    [Required]
    public string FirstName { get; set; } = "";
    [Required]
    public string LastName { get; set; } = "";

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string PasswordConfirm { get; set; }

    public string CitizenId { get; set; }
    public string PhoneNumber { get; set; }
}


public class RegisterByPhoneModel
{
    [Required]
    public string FirstName { get; set; } = "";
    [Required]
    public string LastName { get; set; } = "";

    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string PasswordConfirm { get; set; }

    public string CitizenId { get; set; }

    [Required]
    [Phone]
    public string PhoneNumber { get; set; }
}


public class ConfirmCreateAccountByPhoneModel
{
    [Required]
    public string Code { get; set; }
    [Required]
    public string Key { get; set; } 
}
