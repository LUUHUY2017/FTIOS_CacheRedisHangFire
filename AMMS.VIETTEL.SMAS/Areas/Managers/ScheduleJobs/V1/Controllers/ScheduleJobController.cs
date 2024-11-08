using AMMS.Share.WebApp.Helps;
using Microsoft.AspNetCore.Mvc;

namespace AMMS.VIETTEL.SMAS.Areas.Managers.ScheduleJobs.V1
{
    [Route("v1/[controller]")]
    [AuthorizeMaster]
    public class ScheduleJobController : Controller
    {
        const string pathUrl = "~/Areas/Managers/ScheduleJobs/V1/Views/";

        public IActionResult Index()
        {
            var access_token = Request.Cookies["amms.master.webapp.access_token"];
            return View(pathUrl + "Index.cshtml", access_token);
        }
    }
}
