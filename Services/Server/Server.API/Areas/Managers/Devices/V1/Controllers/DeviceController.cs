﻿using AMMS.Share.WebApp.Helps;
using Microsoft.AspNetCore.Mvc;
using Shared.Core.Identity;

namespace Server.API.Areas.Managers.Devices.V1.Controllers;

[Route("v1/[controller]")]
[AuthorizeMaster(Roles = RoleConst.SuperAdminPage)]
public class DeviceController : Controller
{
    const string pathUrl = "~/Areas/Managers/Devices/V1/Views/";

    public IActionResult Index()
    {
        var access_token = Request.Cookies["amms.master.webapp.access_token"];
        return View(pathUrl + "Index.cshtml", access_token);
    }
}
