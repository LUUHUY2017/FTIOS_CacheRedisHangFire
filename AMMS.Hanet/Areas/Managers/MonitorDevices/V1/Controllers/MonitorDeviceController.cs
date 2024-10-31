using Microsoft.AspNetCore.Mvc;
using Share.WebApp.Controllers;

namespace Server.API.Areas.Managers.MonitorDevices.V1.Controllers;

//[Route("v1/[controller]")]
//[AuthorizeClient]

public class MonitorDeviceController : AuthBaseController
{
    const string pathUrl = "~/Areas/Managers/MonitorDevices/V1/Views/";

    public IActionResult Index()
    {
        var access_token = Request.Cookies["amms.master.webapp.access_token"];
        return View(pathUrl + "Index.cshtml", access_token);
    }
}
