using AMMS.Share.WebApp.Helps;
using Microsoft.AspNetCore.Mvc;

namespace Server.API.Areas.Managers.ScheduleDashBoardReports.V1.Controllers;

[Route("v1/[controller]")]
[AuthorizeMaster(Roles = "Admin")]
public class ScheduleDashBoardReportController : Controller
{
    const string pathUrl = "~/Areas/Managers/ScheduleDashBoardReports/V1/Views/";

    public ScheduleDashBoardReportController()
    {
    }
    public async Task<IActionResult> Index()
    {
        var access_token = Request.Cookies["amms.master.webapp.access_token"];
        return View(pathUrl + "Index.cshtml", access_token);
    }
}