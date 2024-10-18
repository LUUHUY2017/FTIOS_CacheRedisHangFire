
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Server.Core.Identity.Entities;

namespace Server.API.Areas.Managers.Accounts.V1.Controllers
{

    //[Route("[controller]")]
    public class AccountController : Controller
    {
        private string serverUrl = "https://localhost:5001";
        const string pathUrl = "~/Areas/Manage/Accounts/V1/Views/"; 

        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly UserManager<ApplicationUser> _userManager;
        //private readonly IEmailSender _emailSender;

        IList<AuthenticationScheme> ExternalLogins { get; set; }

        public AccountController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager
            //IEmailSender emailSender, 

            )
        {
            _signInManager = signInManager;
            _userManager = userManager;
            //_emailSender = emailSender; 
        }
        [HttpPost("Logout")]
        [HttpGet("Logout")]
        //[ValidateAntiForgeryToken] 
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync();
                await _signInManager.SignOutAsync();
                foreach (var cookie in HttpContext.Request.Cookies)
                    Response.Cookies.Delete(cookie.Key);
            }
            catch (Exception e)
            {

            }
            return Ok();
        }
    }
}
