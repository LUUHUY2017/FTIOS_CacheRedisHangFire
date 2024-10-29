using AMMS.Share.WebApp.Helps;
using Microsoft.AspNetCore.Mvc;
using Shared.Core.Identity;

namespace Server.API.Areas.Reports.TimeAttendenceEvents.CheckInByDay.V1.Controllers;

[Route("v1/[controller]")]
[AuthorizeMaster(Roles = RoleConst.AdminPage)]
public class DeviceAdminController : Controller
{
    const string pathUrl = "~/Areas/Managers/DeviceAdmins/V1/Views/";

    public IActionResult Index()
    {
        var access_token = Request.Cookies["amms.master.webapp.access_token"];
        return View(pathUrl + "Index.cshtml", access_token);
    }
}