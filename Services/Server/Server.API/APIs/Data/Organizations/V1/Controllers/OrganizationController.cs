using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Application.MasterDatas.A2.Organizations.V1;
using Server.Application.MasterDatas.A2.Organizations.V1.Models;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.Lanes.Requests;
using Share.WebApp.Controllers;
using Shared.Core.Commons;

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
    /// Post
    /// </summary>
    /// <returns></returns>
    [HttpPost("Post")]
    public async Task<IActionResult> Post(LaneFilterRequest request)
    {
        try
        {
            var data = await _organizationService.Gets();
            return Ok(data);
        }
        catch (Exception ex)
        {
            return Ok(new Result<List<A2_Lane>>(null, "Lỗi:" + ex.Message, false));
        }
    }




    /// <summary>
    /// Lấy danh sách
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("GetForUser")]
    public async Task<IActionResult> GetForUser()
    {
        _organizationService.UserId = User.GetSubjectId();
        var data = await _organizationService.GetForUser();
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

