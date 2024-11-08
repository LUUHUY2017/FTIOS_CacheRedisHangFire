using AMMS.VIETTEL.SMAS.Applications.Services.AccountVTSmarts.V1;
using AMMS.VIETTEL.SMAS.Applications.Services.AppConfigs.V1;
using AMMS.VIETTEL.SMAS.Applications.Services.AppConfigs.V1.Models;
using AMMS.VIETTEL.SMAS.Cores.Entities.A0;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Share.WebApp.Controllers;
using Shared.Core.Commons;

namespace Server.API.APIs.Data.AttendanceConfigs.V1.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
//[Authorize("Bearer")]
//[AuthorizeMaster(Roles = RoleConst.MasterDataPage)]
public class AppConfigurationController : AuthBaseAPIController
{
    private readonly AppConfigService _attendanceConfigService;
    private readonly AccountVTSmartService _accountVTSmartService;
    public AppConfigurationController(AppConfigService attendanceConfigService, AccountVTSmartService accountVTSmartService)
    {
        _attendanceConfigService = attendanceConfigService;
        _accountVTSmartService = accountVTSmartService;
    }

    [HttpGet("Test")]
    public async Task<IActionResult> Test(string accountName, string password)
    {
        var data = await _accountVTSmartService.PostUserVT(accountName, password);
        return Ok(data);
    }

    [HttpPost("SchoolAsync")]
    public async Task<IActionResult> SchoolAsync(AttendanceConfig model)
    {
        var data = await _attendanceConfigService.SchoolAsync(model);
        return Ok(data);
    }

    /// <summary>
    /// Lấy danh sách
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("Gets")]
    public async Task<IActionResult> Gets()
    {
        var data = await _attendanceConfigService.Gets();
        return Ok(data);
    }

    /// <summary>
    /// Lấy danh sách theo filter
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("GetsFilter")]
    public async Task<IActionResult> GetsFilter(AppConfigFilter filter)
    {
        var data = await _attendanceConfigService.GetsFilter(filter);
        return Ok(data);
    }

    /// <summary>
    /// Lấy cấu hình đầu tiên
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("GetFirstOrDefault")]
    public async Task<IActionResult> GetFirstOrDefault(string orgId)
    {
        var data = await _attendanceConfigService.GetFirstOrDefault(orgId);
        return Ok(data);
    }


    /// <summary>
    /// Tạo hoặc cập nhật cấu hình
    /// </summary>
    /// <returns></returns>
    [HttpPost("AddOrEdit")]
    public async Task<IActionResult> AddOrEdit(AppConfigRequest model)
    {
        return Ok(await _attendanceConfigService.SaveAsync(model));
    }

    /// <summary>
    /// Xóa cấu hình
    /// </summary>
    /// <returns></returns>
    [HttpPost("Delete")]
    public async Task<IActionResult> Delete(DeleteRequest model)
    {
        return Ok(await _attendanceConfigService.DeleteAsync(model));
    }
}
