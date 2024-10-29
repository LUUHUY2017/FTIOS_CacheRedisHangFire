using AMMS.Share.WebApp.Helps;
using Microsoft.AspNetCore.Mvc;
using Shared.Core.Identity;

<<<<<<<< HEAD:Services/Server/Server.API/Areas/Managers/DeviceAdmins/V1/Controllers/DeviceAdminController.cs
namespace Server.API.Areas.Managers.DeviceAdmins.V1.Controllers;

[Route("v1/[controller]")]
[AuthorizeMaster]
public class DeviceAdminController : Controller
{
    const string pathUrl = "~/Areas/Managers/DeviceAdmins/V1/Views/";
========
namespace Server.API.Areas.Reports.TimeAttendenceEvents.CheckInByDay.V1.Controllers;

[Route("v1/[controller]")]
[AuthorizeMaster]
public class CheckInByDayController : Controller
{
    const string pathUrl = "~/Areas/Reports/TimeAttendenceEvents/CheckInByDay/V1/Views/";
>>>>>>>> deverlop:Services/Server/Server.API/Areas/Reports/TimeAttendenceEvents/CheckInByDay/V1/Controllers/CheckInByDayController.cs

    public IActionResult Index()
    {
        var access_token = Request.Cookies["amms.master.webapp.access_token"];
        return View(pathUrl + "Index.cshtml", access_token);
    }
}