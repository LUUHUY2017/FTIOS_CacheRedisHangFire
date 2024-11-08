using AMMS.VIETTEL.SMAS.APIControllers.ScheduleSendMails.V1.Requests;
using AMMS.VIETTEL.SMAS.Cores.Entities.A2;
using AMMS.VIETTEL.SMAS.Cores.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Share.WebApp.Controllers;
using Shared.Core.Commons;

namespace AMMS.VIETTEL.SMAS.APIControllers.ScheduleSendMails.V1.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class ScheduleSendEmailDetailController : AuthBaseAPIController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IScheduleSendEmailDetailRepository _scheduleSendMailDetailRepository;


    public ScheduleSendEmailDetailController(
        IMediator mediator,
        IMapper mapper,
        IScheduleSendEmailDetailRepository scheduleSendEmailDetailRepository
        )
    {
        _mediator = mediator;
        _mapper = mapper;
        _scheduleSendMailDetailRepository = scheduleSendEmailDetailRepository;

    }

    /// <summary>
    /// Lấy chi tiết đăng ký
    /// </summary>
    /// <param name="sheduleId"></param>
    /// <returns></returns>
    /// 
    [HttpGet("Get")]
    public async Task<ActionResult> Gets(string sheduleId)
    {
        var result = await _scheduleSendMailDetailRepository.Get(sheduleId);
        return Ok(result);
    }

    /// <summary>
    /// Cập nhật
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("Edit")]
    public async Task<ActionResult> Edit(ScheduleSendEmailDetailRequest request)
    {
        try
        {
            var model = _mapper.Map<ScheduleSendMailDetail>(request);
            var retVal = await _scheduleSendMailDetailRepository.UpdateAsync(model);
            return Ok(retVal);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    /// <summary>
    ///  Xóa vĩnh viễn
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("Delete")]
    public async Task<ActionResult> Delete([FromBody] DeleteRequest request)
    {
        //request.UserId = UserId;
        var result = await _scheduleSendMailDetailRepository.DeleteAsync(request);
        return Ok(result);
    }


}
