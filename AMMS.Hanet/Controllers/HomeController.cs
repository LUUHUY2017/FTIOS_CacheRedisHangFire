using Microsoft.AspNetCore.Mvc;
using Shared.Core.Loggers;

namespace AMMS.Hanet.Controllers;


public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
