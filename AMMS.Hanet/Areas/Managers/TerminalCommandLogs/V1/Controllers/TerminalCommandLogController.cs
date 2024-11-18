using Microsoft.AspNetCore.Mvc;
using Share.WebApp.Controllers;
using Share.WebApp.Helps;

namespace AMMS.Hanet.Areas.Managers.TerminalCommandLogs.V1.Controllers;

[Route("v1/[controller]")]
[AuthorizeClient]

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
