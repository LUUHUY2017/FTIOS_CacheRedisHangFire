using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace AMMS.ZkAutoPush.Areas.Accounts.V1.Controllers;

public class AccountController : Controller
{
    public AccountController()
    {
    }
    public IActionResult Login(string returnUrl = "/")
    {
        var authenticationProperties = new AuthenticationProperties
        {
            RedirectUri = returnUrl,
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1),
        };
        return Challenge(authenticationProperties);
    }
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("Cookies");
        await HttpContext.SignOutAsync("oidc");

        foreach (var cookie in HttpContext.Request.Cookies)
            Response.Cookies.Delete(cookie.Key);

        return SignOut("Cookies", "oidc");
    }

}
