using Microsoft.AspNetCore.Mvc;
using Shared.Core.Loggers;

namespace AMMS.ZkAutoPush.Controllers;


public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
