using AMMS.VIETTEL.SMAS.Cores.Identity.Entities;

namespace AMMS.VIETTEL.SMAS.Cores.Identity.Interfaces.Accounts.Requests
{
    public class UserAccountRes : ApplicationUser
    {
        public bool? Actived { get; set; }
        public string Password { get; set; } = "";

        public string ConfirmPassword { get; set; } = "";

        public string Phone { get; set; } = "";
        public string CitizenId { get; set; } = "";

        public string? UserType { get; set; } = "";
        public string? CompanyId { get; set; } = "";
        public string? CompanyName { get; set; } = "";
        public string? Descriptions { get; set; } = "";
    }
}
