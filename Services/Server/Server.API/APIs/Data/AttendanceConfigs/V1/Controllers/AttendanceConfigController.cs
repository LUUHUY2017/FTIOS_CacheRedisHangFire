using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Application.MasterDatas.A0.AccountVTSmarts.V1;
using Server.Application.MasterDatas.A0.AttendanceConfigs.V1;
using Server.Application.MasterDatas.A0.AttendanceConfigs.V1.Models;
using Share.WebApp.Controllers;

namespace Server.API.APIs.Data.AttendanceConfigs.V1.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
//[Authorize("Bearer")]
//[AuthorizeMaster(Roles = RoleConst.MasterDataPage)]
public class AttendanceConfigController : AuthBaseAPIController
{
    private readonly AttendanceConfigService _attendanceConfigService;
    private readonly AccountVTSmartService _accountVTSmartService;
    public AttendanceConfigController(AttendanceConfigService attendanceConfigService, AccountVTSmartService accountVTSmartService)
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
    public async Task<IActionResult> AddOrEdit(AttendanceConfigRequest model)
    {
        return Ok(await _attendanceConfigService.SaveAsync(model));
    }
}
