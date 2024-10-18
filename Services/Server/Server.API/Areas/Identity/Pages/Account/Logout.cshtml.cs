using IdentityServer4.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.Core.Loggers;
using Server.Core.Identity.Entities;

namespace AMMS.WebAPI.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class LogoutModel : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IIdentityServerInteractionService _interaction;
    public LogoutModel(SignInManager<ApplicationUser> signInManager, IIdentityServerInteractionService interaction)
    {
        _signInManager = signInManager;
        _interaction = interaction;
    }

    //public void OnGet()
    //{
    //}

    public async Task<IActionResult> OnGet(string returnUrl = null)
    {
        return await this.OnPost(returnUrl);
    }

    public async Task<IActionResult> OnPost(string returnUrl = null)
    {
        await _signInManager.SignOutAsync();
        Logger.Information(string.Format("User logged out."));

        var logoutId = this.Request.Query["logoutId"].ToString();


        if (returnUrl != null)
        {
            return LocalRedirect(returnUrl);
        }
        else if (!string.IsNullOrEmpty(logoutId))
        {
            var logoutContext = await this._interaction.GetLogoutContextAsync(logoutId);
            returnUrl = logoutContext.PostLogoutRedirectUri;

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return this.Redirect(returnUrl);
            }
            else
            {
                return Page();
            }
        }
        else
        {
            return RedirectToPage();
        }
    }
}
