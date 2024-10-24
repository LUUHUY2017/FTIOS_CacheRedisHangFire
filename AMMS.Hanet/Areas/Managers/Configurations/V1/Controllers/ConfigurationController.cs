using AMMS.Hanet.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Share.WebApp.Controllers;
using Share.WebApp.Helps;
using Shared.Core.Loggers;

namespace Server.API.Areas.Managers.Configurations.V1.Controllers;


//[Route("v1/[controller]")]
[AuthorizeClient]
public class ConfigurationController : AuthBaseController
{
    const string pathUrl = "~/Areas/Managers/Configurations/V1/Views/";

    //[HttpGet]
    public IActionResult Index()
    {
        var access_token = Request.Cookies["amms.master.webapp.access_token"];
        return View(pathUrl + "Index.cshtml", access_token);
    }


    public IActionResult Code()
    {
        string url = $"{Request.Scheme}://{Request.Host.Value}";
        string uri = $"https://oauth.hanet.com/oauth2/authorize?response_type=code&scope=full&client_id=152aa3ef5c9c7cef56900c952cabca70&redirect_uri={url}/configuration/token";
        return Redirect(uri);
    }
    public async Task<IActionResult> Token(string code)
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, $"https://oauth.hanet.com/token?grant_type=authorization_code&client_id=152aa3ef5c9c7cef56900c952cabca70&client_secret=a813fc288e01d6eab009ab540cfc9485&code={code}");
        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        string content = await response.Content.ReadAsStringAsync();
        Logger.Warning(content);
        Console.WriteLine(content);

        AccessToken accessToken = JsonConvert.DeserializeObject<AccessToken>(content);
        HanetParam.Token = accessToken;

        string uri = $"{Request.Scheme}://{Request.Host.Value}/configuration";
        return Redirect(uri);
    }


}
