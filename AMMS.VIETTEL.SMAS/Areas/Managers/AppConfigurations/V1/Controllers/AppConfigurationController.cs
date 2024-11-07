using AMMS.Share.WebApp.Helps;
using Microsoft.AspNetCore.Mvc;
using Share.WebApp.Controllers;
using Share.WebApp.Helps;
using Shared.Core.Identity;

namespace AMMS.VIETTEL.SMAS.Areas.Managers.AppConfigurations.V1.Controllers;

[Route("v1/[controller]")]
[AuthorizeClient]
//[AuthorizeMaster(Roles = RoleConst.AdminPage)]
public class AppConfigurationController : AuthBaseController
{
    const string pathUrl = "~/Areas/Managers/AppConfigurations/V1/Views/";

    public IActionResult Index()
    {
        var access_token = Request.Cookies["amms.master.webapp.access_token"];
        return View(pathUrl + "Index.cshtml", access_token);
    }
}
