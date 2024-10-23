using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Application.MasterDatas.A2.Organizations.V1;
using Server.Application.MasterDatas.A2.Organizations.V1.Models;
using Share.WebApp.Controllers;

namespace Server.API.APIs.Data.Organizations.V1.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
//[Authorize("Bearer")]
//[AuthorizeMaster(Roles = RoleConst.MasterDataPage)]
public class OrganizationController : AuthBaseAPIController
{
    private readonly OrganizationService _organizationService;
    public OrganizationController(OrganizationService organizationService)
    {
        _organizationService = organizationService;
    }

    /// <summary>
    /// Lấy danh sách
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("Gets")]
    public async Task<IActionResult> Gets()
    {
        var data = await _organizationService.Gets();
        return Ok(data);
    }

    /// <summary>
    /// Lấy cấu hình trường đầu tiên
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("GetFirstOrDefault")]
    public async Task<IActionResult> GetFirstOrDefault()
    {
        var data = await _organizationService.GetFirstOrDefault();
        return Ok(data);
    }


    /// <summary>
    /// Tạo hoặc cập nhật trường
    /// <returns></returns>
    [HttpPost("AddOrEdit")]
    public async Task<IActionResult> AddOrEdit(OrganizationRequest model)
    {
        return Ok(await _organizationService.SaveAsync(model));
    }
}

