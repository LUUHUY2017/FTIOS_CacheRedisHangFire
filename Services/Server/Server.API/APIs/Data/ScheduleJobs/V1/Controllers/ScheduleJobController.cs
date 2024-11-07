using AutoMapper;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.API.APIs.Data.ScheduleJobs.V1.Requests;
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



    public ScheduleJobController(
        IMediator mediator,
        IMapper mapper,
        IScheduleJobRepository scheduleJobRepository,
        IRecurringJobManager recurringJobManager
        )
    {
        _mediator = mediator;
        _mapper = mapper;
        _scheduleJobRepository = scheduleJobRepository;
        _recurringJobManager = recurringJobManager;

    }

    /// <summary>
    /// Lấy danh sách
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("Gets")]
    public async Task<ActionResult> Filter(ScheduleJobFilter request)
    {
        var model = _mapper.Map<ScheduleJobFilterRequest>(request);
        var data = await _scheduleJobRepository.GetAlls(model);

        //var items = new List<ScheduleJobResponse>();
        //if (data.Any())
        //{
        //    foreach (var item in data)
        //    {
        //        var ite = new ScheduleJobResponse();
        //        CopyProperties.CopyPropertiesTo(item, ite);
        //        ite.ScheduleJobTypeName = ListCategory.ExportType.FirstOrDefault(o => o.Id == item.ScheduleExportType)?.Name;
        //        items.Add(ite);
        //    }
        //}
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
            //if (request.ScheduleReportType == "BAOCAOCHITIETNGAY")
            //    request.ScheduleSequentialSending = "Daily";
            //if (request.ScheduleReportType == "BAOCAOCHITIETTHANG")
            //    request.ScheduleSequentialSending = "Monthly";

            var model = _mapper.Map<ScheduleJob>(request);
            var retVal = await _scheduleJobRepository.UpdateAsync(model);

            //try
            //{
            //    if (retVal.Succeeded)
            //    {
            //        var newCronExpression = "0 * * * *";
            //        var timeSentHour = retVal.Data.ScheduleTimeSend.Value.Hours;
            //        var timeSentMinute = retVal.Data.ScheduleTimeSend.Value.Minutes;

            //        if (request.ScheduleSequentialSending == "Daily" && request.ScheduleNote == "BAOCAOTUDONG")
            //        {
            //            newCronExpression = $"{timeSentMinute} {timeSentHour} * * *";
            //            _IConJobService.UpdateScheduleSendMailCronJob("ScheduleSendMailReportDaily", retVal.Data.Id, newCronExpression);
            //        }
            //        else if (request.ScheduleSequentialSending == "Monthly" && request.ScheduleNote == "BAOCAOTUDONG")
            //        {
            //            newCronExpression = $"{timeSentMinute} {timeSentHour} 1 * *";
            //            _IConJobService.UpdateScheduleSendMailCronJob("ScheduleSendMailReportMonthly", retVal.Data.Id, newCronExpression);
            //        }
            //    }
            //}
            //catch (Exception ex) { }
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
        //if (retVal.Succeeded)
        //{
        //    if (retVal.Data.ScheduleSequentialSending == "Daily" && retVal.Data.ScheduleNote == "BAOCAOTUDONG")
        //        _IConJobService.POC_Report_ScheduleSendMailReportDaily(uId);
        //    else if (retVal.Data.ScheduleSequentialSending == "Monthly" && retVal.Data.ScheduleNote == "BAOCAOTUDONG")
        //        _IConJobService.POC_Report_ScheduleSendMailReportMonthly(uId);
        //}
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
        //if (retVal.Succeeded)
        //{
        //    var newCronExpression = "0 * * * *";
        //    var timeSentHour = retVal.Data.ScheduleTimeSend.Value.Hours;
        //    var timeSentMinute = retVal.Data.ScheduleTimeSend.Value.Minutes;

        //    if (retVal.Data.ScheduleSequentialSending == "Daily" && retVal.Data.ScheduleNote == "BAOCAOTUDONG")
        //    {
        //        newCronExpression = $"{timeSentMinute} {timeSentHour} * * *";
        //        _IConJobService.UpdateScheduleSendMailCronJob("ScheduleSendMailReportDaily", retVal.Data.Id, newCronExpression);
        //    }
        //    else if (retVal.Data.ScheduleSequentialSending == "Monthly" && retVal.Data.ScheduleNote == "BAOCAOTUDONG")
        //    {
        //        newCronExpression = $"{timeSentMinute} {timeSentHour} 1 * *";
        //        _IConJobService.UpdateScheduleSendMailCronJob("ScheduleSendMailReportMonthly", retVal.Data.Id, newCronExpression);
        //    }
        //}
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
        //if (retVal.Succeeded)
        //{
        //    string JobId = "";
        //    if (retVal.Data.ScheduleSequential == "Daily")
        //    {
        //        JobId = $"ScheduleJobDaily" + "_" + retVal.Data.Id;
        //    }
        //    else if (retVal.Data.ScheduleSequential == "Monthly")
        //    {
        //        JobId = $"ScheduleJobMonthly" + "_" + retVal.Data.Id;
        //    }
        //    _recurringJobManager.RemoveIfExists(JobId);
        //}
        return Ok(result);
    }

    #region Options
    /// <summary>
    /// Lấy danh sách tổ chức
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpGet("Organizations")]
    public async Task<ActionResult> Organizations()
    {
        //var items = await _organizationRepository.GetAlls();
        var items = new List<object>();
        return Ok(new { items = items });
    }
    /// <summary>
    /// Lấy danh sách cấu hình
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpGet("ScheduleEmailOptions")]
    public async Task<ActionResult> ScheduleEmailOptions()
    {
        var reportTypes = ListCategory.ReportType.ToList();
        var reportTypesDevice = ListCategory.ReportTypeDevice.ToList();
        var reportTypesInOut = ListCategory.ReportTypeInOut.ToList();
        var sequentialSendings = ListCategory.SequentialSending.ToList();
        var sequentialSendingsDevice = ListCategory.SequentialSendingDevice.ToList();
        var sequentialSendingsInOut = ListCategory.SequentialSendingInOut.ToList();
        var exportTypes = ListCategory.ExportType.ToList();
        var dataCollectTypes = ListCategory.DataCollectType.ToList();
        return Ok(new
        {
            reportTypes = reportTypes,
            reportTypesDevice = reportTypesDevice,
            reportTypesInOut = reportTypesInOut,
            sequentialSendings = sequentialSendings,
            sequentialSendingsDevice = sequentialSendingsDevice,
            sequentialSendingsInOut = sequentialSendingsInOut,
            exportTypes = exportTypes,
            dataCollectTypes = dataCollectTypes
        });
    }
    #endregion

}



