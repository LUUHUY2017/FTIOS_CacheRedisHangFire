using Microsoft.AspNetCore.Mvc;
using Share.WebApp.Controllers;
using Share.WebApp.Helps;

namespace AMMS.VIETTEL.SMAS.Controllers;

[Route("/")]
[AuthorizeClient]
public class HomeController : AuthBaseController
{

    public IActionResult Index()
    {
        var access_token = Request.Cookies["amms.master.webapp.access_token"];
        return View();
    }
}
