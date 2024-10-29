using AMMS.Hanet.Applications.AppConfigs.V1;
using AMMS.Hanet.Applications.AppConfigs.V1.Models;
using AMMS.Hanet.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.Core.Loggers;

namespace AMMS.Hanet.APIControllers.AppConfigs.V1.AppConfigControllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]

public class AppConfigController : ControllerBase
{
    private readonly AppConfigService _appConfigService;
    public AppConfigController(AppConfigService appConfigService)
    {
        _appConfigService = appConfigService;
    }
    /// <summary>
    /// Lấy danh sách
    /// </summary>
    /// <returns></returns>
    [HttpGet("GetFirstOrDefault")]
    public async Task<IActionResult> GetFirstOrDefault()
    {
        return Ok(await _appConfigService.GetFirstOrDefault());
    }

    /// <summary>
    /// Thêm mới hoặc cập nhật cấu hình
    /// </summary>
    /// <returns></returns>
    [HttpPost("AddOrEdit")]
    public async Task<IActionResult> AddOrEdit(AppConfigRequest request)
    {
        return Ok(await _appConfigService.AddOrEdit(request));
    }

    //public IActionResult Code()
    //{
    //    string url = $"{Request.Scheme}://{Request.Host.Value}";
    //    string uri = $"https://oauth.hanet.com/oauth2/authorize?response_type=code&scope=full&client_id=152aa3ef5c9c7cef56900c952cabca70&redirect_uri={url}/configuration/token";
    //    return Redirect(uri);
    //}
    //public async Task<IActionResult> Token(string code)
    //{
    //    var client = new HttpClient();
    //    var request = new HttpRequestMessage(HttpMethod.Post, $"https://oauth.hanet.com/token?grant_type=authorization_code&client_id=152aa3ef5c9c7cef56900c952cabca70&client_secret=a813fc288e01d6eab009ab540cfc9485&code={code}");
    //    var response = await client.SendAsync(request);
    //    response.EnsureSuccessStatusCode();
    //    string content = await response.Content.ReadAsStringAsync();
    //    Logger.Warning(content);
    //    Console.WriteLine(content);

    //    AccessToken accessToken = JsonConvert.DeserializeObject<AccessToken>(content);
    //    HanetParam.Token = accessToken;

    //    string uri = $"{Request.Scheme}://{Request.Host.Value}/configuration";
    //    return Redirect(uri);
    //}
}