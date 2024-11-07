using AutoMapper;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.API.APIs.Data.ScheduleJobs.V1.Requests;
using Server.Application.CronJobs;
using Server.Application.MasterDatas.A2.ScheduleJobs.V1.Models;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.ScheduleJobs;
using Server.Core.Interfaces.A2.ScheduleJobs.Requests;
using Share.WebApp.Controllers;
using Shared.Core.Commons;

namespace Server.API.APIs.Data.ScheduleJobs.V1.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize("Bearer")]
//[AuthorizeMaster(Roles = RoleConst.MasterDataPage)]
public class ScheduleJobController : AuthBaseAPIController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IScheduleJobRepository _scheduleJobRepository;
    private readonly IRecurringJobManager _recurringJobManager;
    private readonly ICronJobService _cronJobService;



    public ScheduleJobController(
        IMediator mediator,
        IMapper mapper,
        IScheduleJobRepository scheduleJobRepository,
        IRecurringJobManager recurringJobManager,
        ICronJobService cronJobService
        )
    {
        _mediator = mediator;
        _mapper = mapper;
        _scheduleJobRepository = scheduleJobRepository;
        _recurringJobManager = recurringJobManager;
        _cronJobService = cronJobService;

    }

    /// <summary>
    /// Lấy danh sách
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("Gets")]
    public async Task<ActionResult> Filter(ScheduleJobFilterRequest request)
    {
        var data = await _scheduleJobRepository.GetAlls(request);

        if (data.Any())
        {
            foreach (var item in data)
            {
                item.ScheduleTypeName = ListScheduleCategory.ScheduleTypes.FirstOrDefault(o => o.Id == item.ScheduleType)?.Name;
                item.ScheduleSequentialName = ListScheduleCategory.Sequentials.FirstOrDefault(o => o.Id == item.ScheduleSequential)?.Name;
            }
        }
        return Ok(new { items = data });
    }

    /// <summary>
    /// Cập nhật
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("Edit")]
    public async Task<ActionResult> Edit(ScheduleJobRequest request)
    {
        try
        {
            var model = _mapper.Map<ScheduleJob>(request);
            var retVal = await _scheduleJobRepository.UpdateAsync(model);

            try
            {
                if (retVal.Succeeded)
                {
                    var scheduleJobs = new List<ScheduleJob> { retVal.Data };
                    await _cronJobService.CreateScheduleCronJob(scheduleJobs);
                }
            }
            catch (Exception ex) { }


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
        var retVal = await _scheduleJobRepository.GetById(id);
        if (retVal.Succeeded)
            await _cronJobService.SyncStudentFromSmas(retVal.Data.Id);
        return Ok();
    }

    /// <summary>
    /// Kích hoạt
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("Active")]
    public async Task<IActionResult> Active([FromBody] ActiveRequest request)
    {
        var result = await _scheduleJobRepository.ActiveAsync(request);
        var retVal = await _scheduleJobRepository.GetById(request.Id);
        if (retVal.Succeeded)
        {
            var scheduleJobs = new List<ScheduleJob> { retVal.Data };
            await _cronJobService.CreateScheduleCronJob(scheduleJobs);
        }
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
        var result = await _scheduleJobRepository.InActiveAsync(request);
        var retVal = await _scheduleJobRepository.GetById(request.Id);

        if (retVal.Succeeded)
        {
            string JobId = $"CronJobSyncFromSmas[*]" + retVal.Data.ScheduleNote;
            await _cronJobService.RemoveScheduleCronJob(JobId, retVal.Data.Id);
        }
        return Ok(result);
    }

    #region Options
    /// <summary>
    /// Lấy danh sách cấu hình
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpGet("ScheduleOptions")]
    public async Task<ActionResult> ScheduleOptions()
    {
        var sequentials = ListScheduleCategory.Sequentials.ToList();
        var scheduleTypes = ListScheduleCategory.ScheduleTypes.ToList();
        return Ok(new
        {
            sequentials = sequentials,
            scheduleTypes = scheduleTypes
        });
    }
    #endregion

}



