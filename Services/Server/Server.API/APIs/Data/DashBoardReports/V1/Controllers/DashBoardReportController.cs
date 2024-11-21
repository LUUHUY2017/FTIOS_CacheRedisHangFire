using AMMS.Notification.Commons;
using AMMS.Share.WebApp.Helps;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Application.CronJobs;
using Server.Application.MasterDatas.A2.DashBoardReports.V1;
using Server.Application.MasterDatas.A2.DashBoardReports.V1.Models;
using Server.Core.Interfaces.A2.DashBoardReports.Models;
using Server.Core.Interfaces.A2.ScheduleSendEmails;
using Share.WebApp.Controllers;
using Shared.Core.Commons;

namespace Server.API.APIs.Data.DashBoardReports.V1.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize("Bearer")]
[AuthorizeMaster]
public class DashBoardReportController : AuthBaseAPIController
{
    private readonly DashBoardReportService _dashBoardReportService;
    private readonly IScheduleSendMailRepository _sheduleSendEmailReponsity;
    private readonly ICronJobService _cronJobService;
    public DashBoardReportController(
        IScheduleSendMailRepository sheduleSendEmailReponsity,
        DashBoardReportService dashBoardReportService,
        ICronJobService cronJobService
        )
    {
        _dashBoardReportService = dashBoardReportService;
        _sheduleSendEmailReponsity = sheduleSendEmailReponsity;
        _cronJobService = cronJobService;
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
        if (retVal.Succeeded)
        {
            await _cronJobService.DashBoard_Report_ScheduleSendMail(id);
        }
        return Ok();
    }

    /// <summary>
    /// Lấy danh sách
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost("Gets")]
    public async Task<ActionResult> Filter(DashBoardReportModel filter)
    {
        var datas = await _dashBoardReportService.Filter(filter);
        return Ok(datas);
    }

    /// <summary>
    /// Cập nhật
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("Edit")]
    public async Task<ActionResult> Edit(DashBoardReportRequest request)
    {
        var datas = await _dashBoardReportService.Edit(request);
        return Ok(datas);
    }

    /// <summary>
    /// Chi tiết lập lịch
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("Detail")]
    public async Task<ActionResult> Detail(string id)
    {
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
        var datas = await _dashBoardReportService.Active(request);
        return Ok(datas);
    }

    /// <summary>
    /// Ngừng kích hoạt
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("Inactive")]
    public async Task<ActionResult> Inactive([FromBody] InactiveRequest request)
    {
        var datas = await _dashBoardReportService.Inactive(request);
        return Ok(datas);
    }
}