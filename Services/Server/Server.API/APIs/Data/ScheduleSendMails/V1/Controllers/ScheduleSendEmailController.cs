using AMMS.Shared.Commons;
using AutoMapper;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Server.API.APIs.Data.ScheduleSendMails.V1.Requests;
using Server.API.APIs.Data.ScheduleSendMails.V1.Responses;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.ScheduleSendEmails;
using Server.Core.Interfaces.A2.ScheduleSendEmails.Requests;
using Share.WebApp.Controllers;
using Shared.Core.Commons;

namespace Server.API.APIs.Data.ScheduleSendMails.V1.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class ScheduleSendEmailController : AuthBaseAPIController
{
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IScheduleSendMailRepository _sheduleSendEmailReponsity;
    //private readonly IOrganizationRepository _organizationRepository;
    //private readonly IConJobService _IConJobService;
    private readonly IRecurringJobManager _recurringJobManager;



    public ScheduleSendEmailController(
        IMediator mediator,
        IMapper mapper,
        IScheduleSendMailRepository sheduleSendEmailReponsity,
        //IOrganizationRepository organizationRepository,
        //IConJobService iConJobService,
        IRecurringJobManager recurringJobManager
        )
    {
        _mediator = mediator;
        _mapper = mapper;
        _sheduleSendEmailReponsity = sheduleSendEmailReponsity;
        //_organizationRepository = organizationRepository;
        //_IConJobService = iConJobService;
        _recurringJobManager = recurringJobManager;

    }

    /// <summary>
    /// Lấy danh sách
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("Gets")]
    public async Task<ActionResult> Filter(ScheduleSendEmailFilter request)
    {
        var model = _mapper.Map<ScheduleSendEmailFilterRequest>(request);
        var data = await _sheduleSendEmailReponsity.GetAlls(model);

        var items = new List<ScheduleSendEmailResponse>();

        if (data.Any())
        {
            foreach (var item in data)
            {
                var ite = new ScheduleSendEmailResponse();
                CopyProperties.CopyPropertiesTo(item, ite);
                ite.ScheduleExportTypeName = ListScheduleEmailCategory.ExportType.FirstOrDefault(o => o.Id == item.ScheduleExportType)?.Name;
                ite.ScheduleReportTypeName = ListScheduleEmailCategory.ReportType.FirstOrDefault(o => o.Id == item.ScheduleReportType)?.Name;
                ite.ScheduleSequentialSendingName = ListScheduleEmailCategory.SequentialSending.FirstOrDefault(o => o.Id == item.ScheduleSequentialSending)?.Name;
                ite.ScheduleDataCollectName = ListScheduleEmailCategory.DataCollectType.FirstOrDefault(o => o.Id == item.ScheduleDataCollect)?.Name;
                items.Add(ite);
            }
        }
        return Ok(new { items = items });
    }

    /// <summary>
    /// Cập nhật
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("Edit")]
    public async Task<ActionResult> Edit(ScheduleSendEmailRequest request)
    {
        try
        {
            if (request.ScheduleReportType == "BAOCAOCHITIETNGAY")
                request.ScheduleSequentialSending = "Daily";
            if (request.ScheduleReportType == "BAOCAOCHITIETTHANG")
                request.ScheduleSequentialSending = "Monthly";

            var model = _mapper.Map<ScheduleSendMail>(request);
            var retVal = await _sheduleSendEmailReponsity.UpdateAsync(model);

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
        var retVal = await _sheduleSendEmailReponsity.GetById(id);
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
        var result = await _sheduleSendEmailReponsity.ActiveAsync(request);
        var retVal = await _sheduleSendEmailReponsity.GetById(request.Id);
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
        var result = await _sheduleSendEmailReponsity.InActiveAsync(request);
        var retVal = await _sheduleSendEmailReponsity.GetById(request.Id);
        if (retVal.Succeeded)
        {
            string JobId = "";
            if (retVal.Data.ScheduleSequentialSending == "Daily")
            {
                JobId = $"ScheduleSendMailReportDaily" + "_" + retVal.Data.Id;
            }
            else if (retVal.Data.ScheduleSequentialSending == "Monthly")
            {
                JobId = $"ScheduleSendMailReportMonthly" + "_" + retVal.Data.Id;
            }
            _recurringJobManager.RemoveIfExists(JobId);
        }
        return Ok(result);
    }

    // <summary>
    ///  Xóa vĩnh viễn
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("Delete")]
    public async Task<ActionResult> Delete(DeleteRequest request)
    {
        var result = await _sheduleSendEmailReponsity.DeleteAsync(request);
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
        var reportTypes = ListScheduleEmailCategory.ReportType.ToList();
        var reportTypesDevice = ListScheduleEmailCategory.ReportTypeDevice.ToList();
        var reportTypesInOut = ListScheduleEmailCategory.ReportTypeInOut.ToList();
        var reportTypesDashBoardReport = ListScheduleEmailCategory.ReportTypeDashBoardReport.ToList();
        var sequentialSendings = ListScheduleEmailCategory.SequentialSending.ToList();
        var sequentialSendingsDevice = ListScheduleEmailCategory.SequentialSendingDevice.ToList();
        var sequentialSendingsInOut = ListScheduleEmailCategory.SequentialSendingInOut.ToList();
        var exportTypes = ListScheduleEmailCategory.ExportType.ToList();
        var dataCollectTypes = ListScheduleEmailCategory.DataCollectType.ToList();
        return Ok(new
        {
            reportTypes = reportTypes,
            reportTypesDevice = reportTypesDevice,
            reportTypesInOut = reportTypesInOut,
            reportTypesDashBoardReport = reportTypesDashBoardReport,
            sequentialSendings = sequentialSendings,
            sequentialSendingsDevice = sequentialSendingsDevice,
            sequentialSendingsInOut = sequentialSendingsInOut,
            exportTypes = exportTypes,
            dataCollectTypes = dataCollectTypes
        });
    }
    #endregion

}



