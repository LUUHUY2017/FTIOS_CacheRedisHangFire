using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public AttendanceConfigController(AttendanceConfigService attendanceConfigService)
    {
        _attendanceConfigService = attendanceConfigService;
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
    public async Task<IActionResult> GetFirstOrDefault()
    {
        var data = await _attendanceConfigService.GetFirstOrDefault();
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
