using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Share.WebApp.Helps;

/// <summary>
/// Filter cho API của các ứng dụng web
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeClientAPIAttribute : AuthorizeAttribute, IAuthorizationFilter, IAuthorizeData //public class AuthorizeCookieAttribute : Attribute, IAuthorizationFilter, IAuthorizeData
{
    public string? Policy { get; set; }
    public string? Roles { get; set; }
    public string? AuthenticationSchemes { get; set; }
    public AuthorizeClientAPIAttribute() : base()
    {

    }
    public AuthorizeClientAPIAttribute(string policy) : base(policy)
    {

    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var queryString = context.HttpContext.Request.QueryString.ToString();
        var returnUrl = context.HttpContext.Request.Path + queryString;

        //var _roleManager = context.HttpContext.RequestServices.GetRequiredService<RoleManager<IdentityRole>>();
        //var _userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();

        var usc = context.HttpContext.User;
        if (usc.Identity.IsAuthenticated)
        {
            var accessToken = context.HttpContext.GetTokenAsync("access_token").GetAwaiter().GetResult();
            context.HttpContext.Items["AMMS_AccessToken"] = accessToken;

            bool isInRole = true;
            if (!string.IsNullOrEmpty(Roles))
            {
                var input_roles = Roles.Trim().Split(",").ToList();
                if (input_roles != null && input_roles.Count() > 0)
                {
                    isInRole = false;
                    foreach (var role in input_roles)
                    {
                        isInRole = usc.IsInRole(role);
                        if (isInRole)
                        {
                            break;
                        }
                    }
                }
            }

            if (!isInRole)
            {
                context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
        else
        {
            context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            //return new ContentResult() { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}

