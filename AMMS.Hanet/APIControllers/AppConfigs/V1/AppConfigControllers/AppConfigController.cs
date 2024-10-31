using AMMS.Hanet.Applications.AppConfigs.V1;
using AMMS.Hanet.Applications.AppConfigs.V1.Models;
using AMMS.Hanet.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Shared.Core.Loggers;

namespace AMMS.Hanet.APIControllers.AppConfigs.V1.Controllers;

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

    [HttpGet("Test")]
    public async Task<IActionResult> Test()
    {
        return Ok(HanetParam.Token);
    }
}