using System.ComponentModel.DataAnnotations;

namespace Server.Core.Identity.Interfaces.Accounts.Requests
{
    public abstract class AddAccountRequest
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }

    public class AddNewAccountRequest : AddAccountRequest
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Phone { get; set; }
        public string UserName { get; set; }
        public bool? LockoutEnabled { get; set; } = true;
    }


    public class AddAccountUsernameRequest : AddAccountRequest
    {

        [EmailAddress]
        public string Email { get; set; }
        public string Phone { get; set; }

        [Required]
        public string UserName { get; set; }
    }

    public class AddAccountEmailRequest : AddAccountRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Phone { get; set; }
    }


    public class AddAccountPhoneRequest : AddAccountRequest
    {
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }
    }

    public class AddAccountCitizenIdRequest : AddAccountRequest
    {
        [EmailAddress]
        public string Email { get; set; }

        public string Phone { get; set; }
        [Required]
        public string CitizenId { get; set; }
    }

}
