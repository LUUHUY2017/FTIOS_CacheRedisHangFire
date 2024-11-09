using Server.Core.Identity.Interfaces.Accounts.Requests;

namespace Server.Core.Identity.Interfaces.Accounts.Services
{
    public interface IAccountService
    {
        public string? UserId { get; set; } 
        ///// <summary>
        ///// Đăng ký tài khoản với email
        ///// </summary>
        ///// <param name="request"></param>
        ///// <param name="origin"></param>
        ///// <returns></returns>
        //Task<Result<string>> RegisterAsync(RegisterRequest request, string origin);

        ///// <summary>
        ///// Xác thực tài khoản qua email
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <param name="code"></param>
        ///// <returns></returns>
        //Task<Result<string>> ConfirmEmailAsync(string userId, string code);

        ///// <summary>
        ///// Khôi phục (quên) mật khẩu
        ///// </summary>
        ///// <param name="model"></param>
        ///// <param name="origin"></param>
        ///// <returns></returns>
        //Task ForgotPassword(ForgotPasswordRequest model, string origin);
        ///// <summary>
        ///// Đặt lại mật khẩu
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //Task<Result<string>> ResetPassword(ResetPasswordRequest model);
        //Task<ApplicationUser> AddAsync(AddNewAccountRequest request);

        ///// <summary>
        ///// Tài xế xe tải đăng ký tài khoản
        ///// </summary>
        ///// <param name="Input"></param>
        ///// <param name="origin"></param>
        ///// <param name="callbackUrl"></param>
        ///// <returns></returns>
        //Task<Result<string>> TruckDriverRegisterAsync(RegisterRequest Input, string origin, string callbackUrl);

        ///// <summary>
        ///// Tạo tài khoản với tên đăng nhập
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //Task<Result<ApplicationUser>> AddAccountUsernameAsync(AddAccountUsernameRequest request);
        ///// <summary>
        ///// Tạo tài khoản với email
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //Task<Result<ApplicationUser>> AddAccountEmailAsync(AddAccountEmailRequest request);
        ///// <summary>
        ///// Tạo tài khoản với số điện thoại
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //Task<Result<ApplicationUser>> AddAccountPhoneAsync(AddAccountPhoneRequest request);


        ///// <summary>
        ///// Tạo tài khoản với CCCD
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //Task<Result<ApplicationUser>> AddAccountCitizenIdAsync(AddAccountCitizenIdRequest request);

        //Task<Result<IdentityResult>> AddToRoleAsync(ApplicationUser user, string role);


        Task<List<UserAccountRes>> GetAccountSystems();
        Task<List<UserAccountRes>> GetAccountSchools();
        Task SendEmailConfirm(string email);
        ////Task<List<UserAccountRes>> GetAccountEmployees();
        ////Task<List<UserAccountRes>> GetAccountVendors();
        ////Task<List<UserAccountRes>> GetAccountCustomers();
        ////Task<List<UserAccountRes>> GetAccountVisiter();

        //Task<List<PageObject>> GetMenuLoginInfo(string id);
        //Task<UserAccountInfoResponse> GetAccountLoginInfo(string id);

    }
}
