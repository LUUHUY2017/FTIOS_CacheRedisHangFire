using AMMS.Share.WebApp.Helps;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Application.MasterDatas.A2.ClassRooms.V1.Models;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.ClassRooms;
using Server.Core.Interfaces.A2.ClassRooms.Requests;
using Share.WebApp.Controllers;
using Shared.Core.Commons;

namespace Server.API.APIs.Data.ClassRooms.V1.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize("Bearer")]
[AuthorizeMaster]
public class ClassRoomController : AuthBaseAPIController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IClassRoomRepository _ClassRoomRepository;



    public ClassRoomController(
        IMediator mediator,
        IMapper mapper,
        IClassRoomRepository ClassRoomRepository
        )
    {
        _mediator = mediator;
        _mapper = mapper;
        _ClassRoomRepository = ClassRoomRepository;

    }



    /// <summary>
    /// Lấy danh sách
    /// </summary>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpGet("Gets")]
    public async Task<IActionResult> Gets()
    {
        var data = await _ClassRoomRepository.Gets();
        var items = new Result<List<ClassRoom>>(data, "Thành công", true);
        return Ok(items);
    }


    /// <summary>
    /// Lấy danh sách
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("Gets")]
    public async Task<ActionResult> Filter(ClassRoomFilterRequest request)
    {
        request.OrganizationId = GetOrganizationId();
        var data = await _ClassRoomRepository.GetAlls(request);
        return Ok(new { items = data });
    }

    /// <summary>
    /// Cập nhật
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("Edit")]
    public async Task<ActionResult> Edit(ClassRoomRequest request)
    {
        try
        {
            var model = _mapper.Map<ClassRoom>(request);
            var retVal = await _ClassRoomRepository.UpdateAsync(model);

            return Ok(retVal);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    /// <summary>
    /// Gửi lại báo cáo
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("Resend")]
    public async Task<ActionResult> Resend(string id)
    {
        var retVal = await _ClassRoomRepository.GetById(id);
        return Ok(new Result<object>("Thành công", true));
    }

    /// <summary>
    /// Kích hoạt
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("Active")]
    public async Task<IActionResult> Active([FromBody] ActiveRequest request)
    {
        var result = await _ClassRoomRepository.ActiveAsync(request);
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
        var result = await _ClassRoomRepository.InActiveAsync(request);
        return Ok(result);
    }


}



