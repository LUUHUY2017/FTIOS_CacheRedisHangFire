using Microsoft.AspNetCore.Mvc;

namespace Server.API.Areas.Managers.AutoReports.Controllers;

public class AutoReportController : Controller
{
    const string pathUrl = "~/Areas/Managers/AutoReports/Views/";

    public IActionResult Index()
    {
        return View(pathUrl + "Index.cshtml");
    }
}
