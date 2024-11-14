using Microsoft.AspNetCore.Mvc;
using Share.WebApp.Controllers;
using Share.WebApp.Helps;

namespace AMMS.ZkAutoPush.Areas.Managers.Configurations.V1;


[AuthorizeClient]
public class ConfigurationController : AuthBaseController
{
    const string pathUrl = "~/Areas/Managers/Configurations/V1/Views/";

    //[HttpGet]
    public IActionResult Index()
    {
        var access_token = Request.Cookies["amms.master.webapp.access_token"];
        return View(pathUrl + "Index.cshtml", access_token);
    }


}
