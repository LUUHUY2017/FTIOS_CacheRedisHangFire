using AMMS.Hanet.Applications.AppConfigs.V1;
using AMMS.Hanet.Applications.AppConfigs.V1.Models;
using AMMS.Hanet.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
    public IActionResult Index()
    {
        ViewData["Message"] = "";
        var access_token = Request.Cookies["amms.master.webapp.access_token"];
        return View(pathUrl + "Index.cshtml", access_token);
    }


    public async Task<IActionResult> Code()
    {
        ViewData["Message"] = "";
        try
        {
            var config = (await _appConfigService.GetFirstOrDefault()).Data;
            string url = $"{Request.Scheme}://{Request.Host.Value}";
            //string uri = $"https://oauth.hanet.com/oauth2/authorize?response_type=code&scope=full&client_id=152aa3ef5c9c7cef56900c952cabca70&redirect_uri={url}/configuration/token";
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
    public async Task<IActionResult> Token(string code)
    {
        ViewData["Message"] = "";
        var access_token = Request.Cookies["amms.master.webapp.access_token"];
        try
        {
            var config = (await _appConfigService.GetFirstOrDefault()).Data;
            var client = new HttpClient();
            //var request = new HttpRequestMessage(HttpMethod.Post, $"https://oauth.hanet.com/token?grant_type=authorization_code&client_id=152aa3ef5c9c7cef56900c952cabca70&client_secret=a813fc288e01d6eab009ab540cfc9485&code={code}");
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://oauth.hanet.com/token?grant_type={config.GrantType}&client_id={config.ClientId}&client_secret={config.ClientScret}&code={code}");
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            #region Lấy không cần xác thực đăng nhập lại
            //var client = new HttpClient();
            //var request = new HttpRequestMessage(HttpMethod.Post, "https://oauth.hanet.com/token");
            //var collection = new List<KeyValuePair<string, string>>();
            //collection.Add(new("grant_type", "refresh_token"));
            //collection.Add(new("client_id", config.ClientId));
            //collection.Add(new("client_secret", config.ClientScret));
            //collection.Add(new("refresh_token", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjQxMjYyMTM0MDE3Mjc3NDY2NDEiLCJlbWFpbCI6Im5hbW5kQGFjcy52biIsImNsaWVudF9pZCI6ImUwZjM0NWRhNWYxODdkMjZiMDE4ZTFkMzYwM2FkOGE4IiwidHlwZSI6InJlZnJlc2hfdG9rZW4iLCJpYXQiOjE3MzAxNzgxNzQsImV4cCI6MTc2MTcxNDE3NH0.fk8cRmNsRwvrsnvm_b7xtZd8tpYhLPATHZ5_S_Xtsbs"));
            //var content = new FormUrlEncodedContent(collection);
            //request.Content = content;
            //var response = await client.SendAsync(request);
            //response.EnsureSuccessStatusCode();
            //string strcontent = await response.Content.ReadAsStringAsync();

            #endregion

            string content = await response.Content.ReadAsStringAsync();
            Logger.Warning(content);
            Console.WriteLine(content);

            AccessToken accessToken = JsonConvert.DeserializeObject<AccessToken>(content);
            HanetParam.Token = accessToken;
            //await _appConfigService.AddOrEdit(new AppConfigRequest(accessToken));
            await _appConfigService.AddOrEdit(new AppConfigRequest(accessToken) { GrantType = config.GrantType, ClientScret = config.ClientScret, ClientId = config.ClientId });

            //string uri = $"{Request.Scheme}://{Request.Host.Value}/configuration";
            //return Redirect(uri);
            ViewData["Message"] = "ok";
            return View(pathUrl + "Index.cshtml", access_token);
        }
        catch(Exception ex)
        {
            ViewData["Message"] = "Thông tin lấy token chưa chính xác. Vui lòng kiểm tra lại!";
            return View(pathUrl + "Index.cshtml", access_token);
        }
       
    }


}
