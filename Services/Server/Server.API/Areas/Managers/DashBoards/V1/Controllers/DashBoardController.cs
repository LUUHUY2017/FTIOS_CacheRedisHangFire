﻿using AMMS.Share.WebApp.Helps;
using Microsoft.AspNetCore.Mvc;

namespace Server.API.Areas.Managers.DashBoards.V1.Controllers;

[Route("v1/[controller]")]
[AuthorizeMaster]
public class DashBoardController : Controller
{
    const string pathUrl = "~/Areas/Managers/DashBoards/V1/Views/";

    public IActionResult Index()
    {
        var access_token = Request.Cookies["amms.master.webapp.access_token"];
        return View(pathUrl + "Index.cshtml", access_token);
    }
}