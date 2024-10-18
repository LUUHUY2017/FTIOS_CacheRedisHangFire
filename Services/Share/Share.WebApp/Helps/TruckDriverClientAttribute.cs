using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;

namespace AMMS.Share.WebApp.Helps;

/// <summary>
/// Filter cho của các ứng dụng web
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class TruckDriverClientAttribute : Attribute, IAuthorizationFilter
{
    public string? Policy { get; set; }
    public string? Roles { get; set; }
    public string? AuthenticationSchemes { get; set; }


    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var queryString = context.HttpContext.Request.QueryString.ToString();
        var returnUrl = context.HttpContext.Request.Path + queryString;
         

        var usc = context.HttpContext.User;
        if (usc.Identity.IsAuthenticated)
        {
            var accessToken = context.HttpContext.GetTokenAsync("access_token").GetAwaiter().GetResult();
            context.HttpContext.Items["AMMS_AccessToken"] = accessToken;
            if(!string.IsNullOrEmpty(accessToken))
            {
                var jwtToken = new JwtSecurityToken(accessToken);
                var accessToken_isExpires = (jwtToken == null) || (jwtToken.ValidFrom > DateTime.UtcNow) || (jwtToken.ValidTo < DateTime.UtcNow);
                if (accessToken_isExpires)
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login", ReturnUrl = returnUrl }));
                }
                else
                {
                    var claims = jwtToken.Claims.ToList();
                    bool isInRole = true;
                    if (!string.IsNullOrEmpty(Roles))
                    {
                        var input_roles = Roles.Trim().Split(",").ToList();
                        if (input_roles != null && input_roles.Count() > 0)
                        {
                            isInRole = false;
                            var roles = claims.Where(o => o.Type == "role").Select(o => o.Value).ToList();
                            foreach (var role in input_roles)
                            {
                                if (roles.Any(o => o == role))
                                {
                                    isInRole = true;
                                    break;
                                }
                            }
                        }
                    }

                    if (!isInRole)
                    {
                        context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Error", ReturnUrl = returnUrl }));
                    }
                }

            }

        }
        else
        {
            // not logged in
            //context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
            context.Result = new RedirectToRouteResult(new Microsoft.AspNetCore.Routing.RouteValueDictionary(new { controller = "Account", action = "Login", ReturnUrl = returnUrl }));
        }

    }
}

