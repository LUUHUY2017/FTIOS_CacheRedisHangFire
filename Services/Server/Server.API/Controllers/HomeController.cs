using AMMS.Share.WebApp.Helps;
using Microsoft.AspNetCore.Mvc;
using Server.API.Models;
using Share.WebApp.Controllers;
using Shared.Core.Identity.Menu;
using System.Diagnostics;

namespace Server.API.Controllers;

[AuthorizeMaster]
public class HomeController : AuthBaseController
{
    public HomeController()
    {
    }

    const string pathUrl = "~/Areas/Managers/Students/V1/Views/";
    const string pathUrl1 = "~/Views/Home/";

    public IActionResult Index()
    {
        var access_token = Request.Cookies["amms.master.webapp.access_token"];
        if (!PagesConst.Menu_MD_Left.Any())
            return View(pathUrl1 + "Index.cshtml", access_token);

        return View(pathUrl + "Index.cshtml", access_token);
    }

    //public IActionResult Index()
    //{
    //    return View();
    //}

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
