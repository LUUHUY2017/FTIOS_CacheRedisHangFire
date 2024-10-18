using AMMS.Share.WebApp.Helps;
using Microsoft.AspNetCore.Mvc;
using Server.API.Models;
using Share.WebApp.Controllers;
using System.Diagnostics;

namespace Server.API.Controllers;

[AuthorizeMaster]
public class HomeController : AuthBaseController
{  
    public HomeController( )
    { 
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
