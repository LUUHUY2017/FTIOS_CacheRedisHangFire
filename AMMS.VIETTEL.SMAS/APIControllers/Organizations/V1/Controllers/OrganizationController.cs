using AMMS.VIETTEL.SMAS.Applications.Services.Organizations.V1.Models;
using AMMS.VIETTEL.SMAS.Cores.Entities;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.Organizations;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.Organizations.Requests;
using AutoMapper;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Application.MasterDatas.A2.Organizations.V1;
using Share.WebApp.Controllers;
using Shared.Core.Commons;

namespace AMMS.VIETTEL.SMAS.APIControllers.Organizations.V1.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
//[Authorize("Bearer")]
//[AuthorizeMaster(Roles = RoleConst.MasterDataPage)]
public class OrganizationController : AuthBaseAPIController
{
    private readonly IMapper _mapper;
    private readonly OrganizationService _organizationService;
    private readonly IOrganizationRepository _organizationRepository;
    public OrganizationController(IMapper mapper, OrganizationService organizationService, IOrganizationRepository organizationRepository)
    {
        _mapper = mapper;
        _organizationService = organizationService;
        _organizationRepository = organizationRepository;
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
    /// Lấy danh sách theo user
    /// </summary>
    /// <returns></returns>
    //[AllowAnonymous]
    [HttpGet("GetForUser")]
    public async Task<IActionResult> GetForUser()
    {
        _organizationService.UserId = User.GetSubjectId();
        //_organizationService.UserId = userId;
        var data = await _organizationService.GetForUser();
        return Ok(data);
    }

    /// <summary>
    /// Post
    /// </summary>
    /// <returns></returns>
    [HttpPost("Post")]
    public async Task<IActionResult> Post(OrganizationFilterRequest request)
    {
        var data = await _organizationRepository.GetAlls(request);
        return Ok(data);

    }
    /// <summary>
    /// Cập nhật
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("Edit")]
    public async Task<ActionResult> Edit(OrganizationRequest request)
    {
        try
        {
            var model = _mapper.Map<Organization>(request);
            var retVal = await _organizationRepository.UpdateAsync(model);

            return Ok(retVal);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    /// <summary>
    /// Kích hoạt
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("Active")]
    public async Task<IActionResult> Active([FromBody] ActiveRequest request)
    {
        var result = await _organizationRepository.ActiveAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// Ngừng kích hoạt
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("Inactive")]
    public async Task<ActionResult> Inactive([FromBody] InactiveRequest request)
    {
        var result = await _organizationRepository.InActiveAsync(request);
        return Ok(result);
    }

}

