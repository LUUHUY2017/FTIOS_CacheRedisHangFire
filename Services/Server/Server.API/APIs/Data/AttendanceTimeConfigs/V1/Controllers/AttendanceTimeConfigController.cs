using AMMS.Share.WebApp.Helps;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.API.APIs.Data.AttendanceTimeConfigs.V1.Models;
using Server.Application.MasterDatas.A0.AttendanceTimeConfigs.V1;
using Server.Application.MasterDatas.A0.AttendanceTimeConfigs.V1.Models;
using Server.Application.MasterDatas.A2.Devices;
using Share.WebApp.Controllers;
using Shared.Core.Commons;

namespace Server.API.APIs.Data.AttendanceTimeConfigs.V1.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize("Bearer")]
[AuthorizeMaster]
public class AttendanceTimeConfigController : AuthBaseAPIController
{
    private readonly AttendanceTimeConfigService _attendanceTimeConfigService;
    public AttendanceTimeConfigController(AttendanceTimeConfigService attendanceTimeConfigService)
    {
        _attendanceTimeConfigService = attendanceTimeConfigService;
    }


    /// <summary>
    /// Lấy danh sách type
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("GetTypes")]
    public async Task<IActionResult> GetTypes()
    {
        return Ok(new Result<List<object>>(AttendanceTimeTypeConst.AttendanceTimeTypes, "Thành công", true));
    }

    /// <summary>
    /// Lấy danh sách
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("Gets")]
    public async Task<IActionResult> Gets()
    {
        var data = await _attendanceTimeConfigService.Gets();
        return Ok(data);
    }

    /// <summary>
    /// Lấy danh sách có filter
    /// </summary>
    /// <returns></returns>
    [HttpPost("GetsFilter")]
    public async Task<IActionResult> GetsFilter(AttendanceTimeConfigFilter filter)
    {
            _attendanceTimeConfigService.UserId = User.GetSubjectId();
        var data = await _attendanceTimeConfigService.GetsFilter(filter);
        return Ok(data);
    }

    /// <summary>
    /// Lấy cấu hình đầu tiên
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("GetFirstOrDefault")]
    public async Task<IActionResult> GetFirstOrDefault(string? orgId)
    {
        var data = await _attendanceTimeConfigService.GetFirstOrDefault(orgId);
        return Ok(data);
    }


    /// <summary>
    /// Tạo hoặc cập nhật cấu hình
    /// </summary>
    /// <returns></returns>
    [HttpPost("AddOrEdit")]
    public async Task<IActionResult> AddOrEdit(AttendanceTimeConfigRequest model)
    {
        return Ok(await _attendanceTimeConfigService.SaveAsync(model));
    }

    /// <summary>
    /// Tạo hoặc cập nhật cấu hình
    /// </summary>
    /// <returns></returns>
    [HttpPost("Delete")]
    public async Task<IActionResult> Delete(DeleteRequest request)
    {
        return Ok(await _attendanceTimeConfigService.DeleteAsync(request));
    }
    /// <summary>
    /// Tắt hoạt động 
    /// </summary>
    /// <returns></returns>
    [HttpPost("InActive")]
    public async Task<IActionResult> Active(ActiveRequest request)
    {
        return Ok(await _attendanceTimeConfigService.Active(request));
    }

    /// <summary>
    /// Bật hoạt động 
    /// </summary>
    /// <returns></returns>
    [HttpPost("Active")]
    public async Task<IActionResult> InActive(InactiveRequest request)
    {
        return Ok(await _attendanceTimeConfigService.InActive(request));
    }

}