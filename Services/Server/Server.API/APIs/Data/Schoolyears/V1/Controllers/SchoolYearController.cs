using AMMS.Share.WebApp.Helps;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Application.MasterDatas.A2.SchoolYears.V1.Models;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.SchoolYears;
using Server.Core.Interfaces.A2.SchoolYears.Requests;
using Share.WebApp.Controllers;
using Shared.Core.Commons;

namespace Server.API.APIs.Data.Schoolyears.V1.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize("Bearer")]
[AuthorizeMaster]
public class SchoolYearController : AuthBaseAPIController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ISchoolYearRepository _SchoolYearRepository;



    public SchoolYearController(
        IMediator mediator,
        IMapper mapper,
        ISchoolYearRepository SchoolYearRepository
        )
    {
        _mediator = mediator;
        _mapper = mapper;
        _SchoolYearRepository = SchoolYearRepository;

    }

    /// <summary>
    /// Lấy danh sách
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("Gets")]
    public async Task<ActionResult> Filter(SchoolYearFilterRequest request)
    {

        var data = await _SchoolYearRepository.GetAlls(request);
        return Ok(new { items = data });
    }

    /// <summary>
    /// Cập nhật
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("Edit")]
    public async Task<ActionResult> Edit(SchoolYearRequest request)
    {
        try
        {
            var model = _mapper.Map<SchoolYear>(request);
            var retVal = await _SchoolYearRepository.UpdateAsync(model);

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
        var result = await _SchoolYearRepository.ActiveAsync(request);
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
        var result = await _SchoolYearRepository.InActiveAsync(request);
        return Ok(result);
    }


}



