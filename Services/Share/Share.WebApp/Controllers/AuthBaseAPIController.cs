using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Share.WebApp.Controllers;

public abstract class AuthBaseAPIController : ControllerBase
{
    public AuthBaseAPIController() : base()
    {

    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public AccountInfo GetAccountInfo()
    {
        var account = new AccountInfo();
        if (User != null || User.Identity.IsAuthenticated)
        {
            var claims = User.Claims.ToList();
            account.UserId = claims.FirstOrDefault(o => o.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value.ToString();
            var email = claims.FirstOrDefault(o => o.Type == "email");
            account.Email = email == null ? "" : email.Value.ToString();
            var phone = claims.FirstOrDefault(o => o.Type == "phone");
            account.Phone = phone == null ? "" : phone.Value.ToString();
            var userName = claims.FirstOrDefault(o => o.Type == "username");
            account.UserName = userName == null ? "" : userName.Value.ToString();
            var fullname = claims.FirstOrDefault(o => o.Type == "fullname");
            account.FullName = fullname == null ? "" : fullname.Value.ToString();
        }
        return account;
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public string GetUserId()
    {
        if (User == null)
        {
            return "";
        }
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            userId = User.FindFirstValue("sub");
        }
        return userId;
    }


    public string UserId
    {
        get => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue("sub");
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public string GetAccessToken()
    {
        var acccesToken = Request.Cookies["amms.master.webapp.access_token"];
        return acccesToken;
    }

    [ApiExplorerSettings(IgnoreApi = true)]
    public string GetOrganizationId()
    {
        var cookieValue = Request.Cookies["organizationId"];
        return cookieValue;
    }

    //public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext filterContext)
    //{
    //    BaseParam = new BaseParamModel()
    //    {
    //        AMMS_AccessToken = AMMS_AccessToken,
    //        AMMS_MasterHostAddress = AMMS_Master_HostAddress,
    //    };

    //    base.OnActionExecuting(filterContext);
    //}
}

public class AccountInfo
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }

}