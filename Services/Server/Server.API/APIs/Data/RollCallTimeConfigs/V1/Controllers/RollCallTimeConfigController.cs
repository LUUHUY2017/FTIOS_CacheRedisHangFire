using AMMS.Share.WebApp.Helps;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Application.MasterDatas.A0.RollCallTimeConfigs.V1;
using Server.Application.MasterDatas.A0.RollCallTimeConfigs.V1.Models;
using Share.WebApp.Controllers;
using Shared.Core.Commons;

namespace Server.API.APIs.Data.RollCallTimeConfigs.V1.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize("Bearer")]
[AuthorizeMaster]
public class RollCallTimeConfigController : AuthBaseAPIController
{
    private readonly RollCallTimeConfigService _rollCallTimeConfigService;
    public RollCallTimeConfigController(RollCallTimeConfigService rollCallTimeConfigService)
    {
        _rollCallTimeConfigService = rollCallTimeConfigService;
    }

    /// <summary>
    /// Lấy danh sách
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("Gets")]
    public async Task<IActionResult> Gets()
    {
        var data = await _rollCallTimeConfigService.Gets();
        return Ok(data);
    }

    /// <summary>
    /// Lấy danh sách có filter
    /// </summary>
    /// <returns></returns>
    [HttpPost("GetsFilter")]
    public async Task<IActionResult> GetsFilter(RollCallTimeConfigFilter filter)
    {
        _rollCallTimeConfigService.UserId = User.GetSubjectId();
        var data = await _rollCallTimeConfigService.GetsFilter(filter);
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
        var data = await _rollCallTimeConfigService.GetFirstOrDefault(orgId);
        return Ok(data);
    }


    /// <summary>
    /// Tạo hoặc cập nhật cấu hình
    /// </summary>
    /// <returns></returns>
    [HttpPost("AddOrEdit")]
    public async Task<IActionResult> AddOrEdit(RollCallTimeConfigRequest model)
    {
        return Ok(await _rollCallTimeConfigService.SaveAsync(model));
    }

    /// <summary>
    /// Tạo hoặc cập nhật cấu hình
    /// </summary>
    /// <returns></returns>
    [HttpPost("Delete")]
    public async Task<IActionResult> Delete(DeleteRequest request)
    {
        return Ok(await _rollCallTimeConfigService.DeleteAsync(request));
    }
}