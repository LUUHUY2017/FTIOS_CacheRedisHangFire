using Microsoft.AspNetCore.Mvc;
using Share.WebApp.Controllers;
using Share.WebApp.Helps;

namespace AMMS.ZkAutoPush.Controllers;

[AuthorizeClient]
public class HomeController : AuthBaseController
{
    public IActionResult Index()
    {
        return View();
    }
}
