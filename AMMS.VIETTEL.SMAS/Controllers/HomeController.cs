using Microsoft.AspNetCore.Mvc;
using Share.WebApp.Controllers;

namespace AMMS.VIETTEL.SMAS.Controllers;

[Route("[controller]")]
//[AuthorizeClient]
public class HomeController : AuthBaseController
{
    public IActionResult Index()
    {
        return View();
    }
}
