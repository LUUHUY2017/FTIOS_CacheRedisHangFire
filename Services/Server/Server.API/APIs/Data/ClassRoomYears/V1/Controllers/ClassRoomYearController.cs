using AMMS.Share.WebApp.Helps;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Application.MasterDatas.A2.ClassRoomYears.V1.Models;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.StudentClassRoomYears;
using Server.Core.Interfaces.A2.StudentClassRoomYears.Requests;
using Share.WebApp.Controllers;
using Shared.Core.Commons;

namespace Server.API.APIs.Data.ClassRoomYears.V1.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize("Bearer")]
[AuthorizeMaster]
public class ClassRoomYearController : AuthBaseAPIController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IStudentClassRoomYearRepository _ClassRoomYearRepository;



    public ClassRoomYearController(
        IMediator mediator,
        IMapper mapper,
        IStudentClassRoomYearRepository ClassRoomRepository
        )
    {
        _mediator = mediator;
        _mapper = mapper;
        _ClassRoomYearRepository = ClassRoomRepository;

    }

    /// <summary>
    /// Lấy danh sách
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("Gets")]
    public async Task<ActionResult> Filter(ClassRoomYearFilterRequest request)
    {
        request.OrganizationId = GetOrganizationId();
        var data = await _ClassRoomYearRepository.GetAlls(request);
        return Ok(new { items = data });
    }

    /// <summary>
    /// Cập nhật
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("Edit")]
    public async Task<ActionResult> Edit(ClassRoomYearRequest request)
    {
        try
        {
            var model = _mapper.Map<StudentClassRoomYear>(request);
            request.SchoolId = request.OrganizationId;
            var retVal = await _ClassRoomYearRepository.UpdateAsync(model);

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
        var result = await _ClassRoomYearRepository.ActiveAsync(request);
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
        var result = await _ClassRoomYearRepository.InActiveAsync(request);
        return Ok(result);
    }


}



