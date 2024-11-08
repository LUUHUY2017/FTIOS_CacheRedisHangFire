using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Server.API.APIs.Data.Users.V1.Commons;
using Server.Application.MasterDatas.A0.AccountVTSmarts.V1;
using Server.Core.Entities.A0;
using Server.Core.Entities.A2;
using Server.Core.Identity.Entities;
using Server.Core.Interfaces.A2.Organizations;
using Server.Infrastructure.Datas.MasterData;
using Share.WebApp.Controllers;
using Shared.Core.Identity;
using Shared.Core.Loggers;
using System.ComponentModel.DataAnnotations;

namespace AMMS.WebAPI.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class LoginModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly AccountVTSmartService _accountVTSmartService;
    private readonly MasterDataDbContext _masterDataDbContext;
    private readonly IOrganizationRepository _organizationRepository;
    public LoginModel(SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration,
        AccountVTSmartService accountVTSmartService,
        MasterDataDbContext masterDataDbContext,
        IOrganizationRepository organizationRepository)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _accountVTSmartService = accountVTSmartService;
        _masterDataDbContext = masterDataDbContext;
        _organizationRepository = organizationRepository;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public IList<AuthenticationScheme> ExternalLogins { get; set; }

    public string ReturnUrl { get; set; }

    [TempData]
    public string ErrorMessage { get; set; }

    public class InputModel
    {
        [Required]
        //[EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public async Task OnGetAsync(string returnUrl = null)
    {
        if (!string.IsNullOrEmpty(ErrorMessage))
        {
            ModelState.AddModelError(string.Empty, ErrorMessage);
        }


        returnUrl ??= Url.Content("~/");

        // Clear the existing external cookie to ensure a clean login process
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        ReturnUrl = returnUrl;
    }
    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        if (ModelState.IsValid)
        {
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var user = await _userManager.FindByNameAsync(Input.Email) ?? await _userManager.FindByEmailAsync(Input.Email);
            if (user != null && !user.EmailConfirmed)
            {
                ModelState.AddModelError(string.Empty, "Account not confirmed!");
                return Page();
            }
            var result = await _signInManager.PasswordSignInAsync(user?.UserName, Input.Password, Input.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                if (!returnUrl.Contains("/connect/authorize/callback?client_id"))
                {
                    var acctoken = await AccountService.Login_Password(AuthBaseController.AMMS_Master_HostAddress, "amms.master.webapp", "secret", Input.Email, Input.Password);

                    if (acctoken.Succeeded)
                    {
                        HttpContext.Response.Cookies.Append("amms.master.webapp.access_token", acctoken.Data.access_token);
                        HttpContext.Response.Cookies.Append("amms.master.webapp.refresh_token", acctoken.Data.refresh_token);
                        HttpContext.Response.Cookies.Append("amms.master.webapp.expires_in", acctoken.Data.expires_in.ToString());
                        HttpContext.Response.Cookies.Append("amms.master.webapp.scope", acctoken.Data.scope);
                    }
                }

                Logger.Information(string.Format("User logged in."));
                return LocalRedirect(returnUrl);
            }
            if (result.RequiresTwoFactor)
            {
                return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
            }
            if (result.IsLockedOut)
            {
                Logger.Information(string.Format("User account locked out."));
                return RedirectToPage("./Lockout");
            }
            else
            {

                //try
                //{
                //    //Qua VT để lấy token tk: Input.Email, Input.Password
                    
                //    var response = await _accountVTSmartService.PostUserVT(Input.Email, Input.Password);
                //    var a = 1;
                //    if (response.currentUser != null && response.currentTenant != null)
                //    {
                //        // Xử lý trường hợp thành công: hệ thống VT có tài khoản

                //        // Kiểm tra school
                //        var checkSchool = await _masterDataDbContext.Organization
                //                                .FirstOrDefaultAsync(x => x.OrganizationName.ToLower().Trim() == response.currentTenant.name.ToLower().Trim());
                //        if (checkSchool == null)
                //        {
                //            // Trường hợp trường chưa được khai báo
                //            var newSchool = new Organization()
                //            {
                //                Id = response.currentUser.id.ToString(),
                //                OrganizationName = response.currentTenant?.name,
                //            };
                //            var addSchoolRes = await _organizationRepository.AddAsync(newSchool);
                //            checkSchool = addSchoolRes.Data;
                //        }

                //        //Tạo tài khoản ở hệ thống mới và xác thực lại
                //        var userNew = new ApplicationUser
                //        {
                //            Id = response.currentUser.id.ToString(),
                //            UserName = Input.Email,
                //            Email = Input.Email,
                //            PhoneNumber = response.currentUser.phoneNumber,
                //            FirstName = response.currentUser.userName.ToString(),
                //            Type = UserTypeConst.User,
                //            OrganizationId = checkSchool.Id,
                //        };
                //        var result1 = await _userManager.CreateAsync(userNew, Input.Password);

                //        if (result1.Succeeded)
                //        {
                //            //Gắn RoleGroup cho user
                //            var roleManager = await _masterDataDbContext.RoleGroup.FirstOrDefaultAsync(x => x.Name == "Manager");
                //            await _masterDataDbContext.RoleGroupUser.AddAsync(new RoleGroupUser()
                //            {
                //                UserId = userNew.Id,
                //                RoleGroupId = roleManager?.Id,
                //            });
                //            _masterDataDbContext.SaveChanges();
                //            var user2 = await _userManager.FindByNameAsync(Input.Email) ?? await _userManager.FindByEmailAsync(Input.Email);
                //            var result2 = await _signInManager.PasswordSignInAsync(user?.UserName, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                //            if (result2.Succeeded)
                //            {
                //                if (!returnUrl.Contains("/connect/authorize/callback?client_id"))
                //                {
                //                    var acctoken = await AccountService.Login_Password(AuthBaseController.AMMS_Master_HostAddress, "amms.master.webapp", "secret", Input.Email, Input.Password);

                //                    if (acctoken.Succeeded)
                //                    {
                //                        HttpContext.Response.Cookies.Append("amms.master.webapp.access_token", acctoken.Data.access_token);
                //                        HttpContext.Response.Cookies.Append("amms.master.webapp.refresh_token", acctoken.Data.refresh_token);
                //                        HttpContext.Response.Cookies.Append("amms.master.webapp.expires_in", acctoken.Data.expires_in.ToString());
                //                        HttpContext.Response.Cookies.Append("amms.master.webapp.scope", acctoken.Data.scope);
                //                    }
                //                }

                //                Logger.Information(string.Format("User logged in."));
                //                return LocalRedirect(returnUrl);
                //            }

                //            if (result.RequiresTwoFactor)
                //            {
                //                return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                //            }
                //            if (result.IsLockedOut)
                //            {
                //                Logger.Information(string.Format("User account locked out."));
                //                return RedirectToPage("./Lockout");
                //            }
                //        }

                //    }
                //}
                //catch (Exception ex)
                //{
                //    Logger.Error(ex);
                //}
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }
        }

        // If we got this far, something failed, redisplay form
        return Page();
    }

}
