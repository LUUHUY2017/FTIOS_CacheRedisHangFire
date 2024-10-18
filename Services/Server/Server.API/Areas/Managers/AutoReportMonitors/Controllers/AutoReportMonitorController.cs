﻿using AMMS.Share.WebApp.Helps;
using Microsoft.AspNetCore.Mvc;

namespace Server.API.Areas.Managers.AutoReportMonitors.Controllers;

[AuthorizeMaster]

public class AutoReportMonitorController : Controller
{
    const string pathUrl = "~/Areas/Managers/AutoReportMonitors/Views/";

    public IActionResult Index()
    {
        var access_token = Request.Cookies["amms.master.webapp.access_token"];

        return View(pathUrl + "Index.cshtml", access_token);
    }
}
