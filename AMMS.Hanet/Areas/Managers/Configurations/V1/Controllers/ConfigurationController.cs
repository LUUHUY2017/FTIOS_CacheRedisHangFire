using AMMS.Hanet.Applications.AppConfigs.V1;
using AMMS.Hanet.Applications.AppConfigs.V1.Models;
using AMMS.Hanet.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using Share.WebApp.Controllers;
using Share.WebApp.Helps;
using Shared.Core.Loggers;

namespace Server.API.Areas.Managers.Configurations.V1.Controllers;


//[Route("v1/[controller]")]
//[AuthorizeClient]
public class ConfigurationController : AuthBaseController
{
    const string pathUrl = "~/Areas/Managers/Configurations/V1/Views/";
    private readonly AppConfigService _appConfigService;
    public ConfigurationController(AppConfigService appConfigService)
    {
        _appConfigService = appConfigService;
    }
    //[HttpGet]
    public async Task<IActionResult> Index()
    {
        ViewData["Message"] = "";
        var config = (await _appConfigService.GetFirstOrDefault()).Data;
        return View(pathUrl + "Index.cshtml", config);
    }


    public async Task<IActionResult> Code()
    {
        ViewData["Message"] = "";
        try
        {
            var config = (await _appConfigService.GetFirstOrDefault()).Data;
            string url = $"{Request.Scheme}://{Request.Host.Value}";
            string uri = $"https://oauth.hanet.com/oauth2/authorize?response_type=code&scope=full&client_id={config.ClientId}&redirect_uri={url}/configuration/token";
            return Redirect(uri);
        }
        catch (Exception ex)
        {
            var access_token = Request.Cookies["amms.master.webapp.access_token"];
            ViewData["Message"] = "Có lỗi trong quá trình lấy Token";
            return View(pathUrl + "Index.cshtml", access_token);
        }

    }
    /// <summary>
    /// client_id=152aa3ef5c9c7cef56900c952cabca70
    /// client_secret=a813fc288e01d6eab009ab540cfc9485
    /// grant_type=authorization_code
    /// </summary>
    public async Task<IActionResult> Token()
    {
        ViewData["Message"] = "";
        var access_token = Request.Cookies["amms.master.webapp.access_token"];
        try
        {
           
            //string uri = $"{Request.Scheme}://{Request.Host.Value}/configuration";
            //return Redirect(uri);
            ViewData["Message"] = "ok";
            return View(pathUrl + "Index.cshtml", access_token);
        }
        catch (Exception ex)
        {
            ViewData["Message"] = "Thông tin lấy token chưa chính xác. Vui lòng kiểm tra lại!";
            return View(pathUrl + "Index.cshtml", access_token);
        }

    }


}
