using Microsoft.AspNetCore.Mvc;
using Share.WebApp.Controllers;

namespace AMMS.ZkAutoPush.Areas.Managers.TerminalCommandLogs.V1;

[Route("v1/[controller]")]
//[AuthorizeClient]

public class TerminalCommandLogController : AuthBaseController
{
    const string pathUrl = "~/Areas/Managers/TerminalCommandLogs/V1/Views/";
    //const string pathUrl = "~/Areas/Managers/TerminalCommandLogs/";

    public IActionResult Index()
    {
        var access_token = Request.Cookies["amms.master.webapp.access_token"];
        return View(pathUrl + "Index.cshtml", access_token);
    }
}
