using System.ComponentModel.DataAnnotations;

namespace Server.API.APIs.Data.Users.V1.Models
{
    public class ChangePhoneByOTP
    {
        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        public string Otp { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Key { get; set; }
    }
    public class ChangeEmailByOTP
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Otp { get; set; }
        [Required]
        public string Code { get; set; }

        [Required]
        public string Key { get; set; }
    }
    public class ChangeEmailByOTPResponse
    {
        public string Code { get; set; }
        public string Key { get; set; }
    }

    public class SetPasswordModel
    {
        public string UserId { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

}
