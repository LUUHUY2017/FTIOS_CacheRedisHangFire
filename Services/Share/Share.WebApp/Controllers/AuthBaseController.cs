using AMMS.Share.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace Share.WebApp.Controllers;


/// <summary>
/// Lớp quản lý 
/// </summary>
public abstract class AuthBaseController : Controller
{
    public static string AMMS_Master_HostAddress { get; set; }
    public BaseParamModel BaseParam { get; private set; }


    public string AMMS_AccessToken
    {
        get
        {
            if (HttpContext != null)
                return (string)HttpContext.Items["AMMS_AccessToken"];
            return null;
        }
    }

    public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext filterContext)
    {
        BaseParam = new BaseParamModel()
        {
            AMMS_AccessToken = AMMS_AccessToken,
            AMMS_MasterHostAddress = AMMS_Master_HostAddress,
        };

        base.OnActionExecuting(filterContext);
    }
    public AuthBaseController() : base()
    {
    }
}
