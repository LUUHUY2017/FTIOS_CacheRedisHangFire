using AutoMapper;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Application.CronJobs;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.ScheduleJobs;
using Share.WebApp.Controllers;
using Shared.Core.Commons;

namespace Server.API.APIs.Data.ScheduleJobs.V1.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize("Bearer")]
//[AuthorizeMaster(Roles = RoleConst.MasterDataPage)]
public class ScheduleJobLogController : AuthBaseAPIController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IScheduleJobLogRepository _scheduleJobLogRepository;
    private readonly IRecurringJobManager _recurringJobManager;
    private readonly ICronJobService _cronJobService;



    public ScheduleJobLogController(
        IMediator mediator,
        IMapper mapper,
        IScheduleJobLogRepository scheduleJobLogRepository,
        IRecurringJobManager recurringJobManager,
        ICronJobService cronJobService
        )
    {
        _mediator = mediator;
        _mapper = mapper;
        _scheduleJobLogRepository = scheduleJobLogRepository;
        _recurringJobManager = recurringJobManager;
        _cronJobService = cronJobService;

    }

    /// <summary>
    /// Lấy  chi tiết
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpGet("GetByScheduleJobId")]
    public async Task<ActionResult> GetByScheduleJobId(string scheduleJobId)
    {
        var data = await _scheduleJobLogRepository.GetByScheduleJobId(scheduleJobId);
        return Ok(new Result<object>(data, "Thành công!", true));
    }



    /// <summary>
    /// Cập nhật
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("Edit")]
    public async Task<ActionResult> Edit(ScheduleJobLog request)
    {
        try
        {
            var retVal = await _scheduleJobLogRepository.UpdateAsync(request);
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
        var result = await _scheduleJobLogRepository.ActiveAsync(request);
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
        var result = await _scheduleJobLogRepository.InActiveAsync(request);
        return Ok(result);
    }



}



