﻿using Microsoft.AspNetCore.Mvc;
using Share.WebApp.Helps;

namespace AMMS.VIETTEL.SMAS.Areas.Managers.StudentSmas.V1.Controllers
{
    [Route("v1/[controller]")]
    [AuthorizeClient]
    public class StudentSmasController : Controller
    {
        const string pathUrl = "~/Areas/Managers/StudentSmas/V1/Views/";

        public IActionResult Index()
        {
            var access_token = Request.Cookies["amms.master.webapp.access_token"];
            return View(pathUrl + "Index.cshtml", access_token);
        }
    }
}