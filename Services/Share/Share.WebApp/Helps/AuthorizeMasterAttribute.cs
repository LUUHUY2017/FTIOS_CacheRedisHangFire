using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;

namespace AMMS.Share.WebApp.Helps;


/// <summary>
/// Kiểm soát truy cập bên Masster
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeMasterAttribute : Attribute, IAuthorizationFilter, IAuthorizeData
{
    public string? Policy { get; set; }
    public string? Roles { get; set; }
    public string? AuthenticationSchemes { get; set; }

    //public AuthorizeMasterAttribute()
    //{
    //}
    //public AuthorizeMasterAttribute(string? policy)
    //{
    //    Policy = policy;
    //}

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var queryString = context.HttpContext.Request.QueryString.ToString();
        var returnUrl = context.HttpContext.Request.Path + queryString;


        var usc = context.HttpContext.User;

        if (usc.Identity.IsAuthenticated)
        {
            var accessToken = context.HttpContext.Request.Cookies["amms.master.webapp.access_token"];
            context.HttpContext.Items["AMMS_AccessToken"] = accessToken;
            if (!string.IsNullOrEmpty(accessToken))
            {
                var jwtToken = new JwtSecurityToken(accessToken);
                var accessToken_isExpires = (jwtToken == null) || (jwtToken.ValidFrom > DateTime.UtcNow) || (jwtToken.ValidTo < DateTime.UtcNow);
                if (accessToken_isExpires)
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "Login", ReturnUrl = returnUrl }));
                }
                else
                {
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