using AMMS.Share.WebApp.Helps;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Application.MasterDatas.A2.DashBoards.V1;
using Server.Application.MasterDatas.A2.DashBoards.V1.Models;
using Server.Application.MasterDatas.A2.DashBoards.V1.Models.Devices;
using Share.WebApp.Controllers;

namespace Server.API.APIs.Data.DashBoards.V1.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize("Bearer")]
[AuthorizeMaster]
public class DashBoardController : AuthBaseAPIController
{
    private readonly DashBoardService _dashBoardService;

    public DashBoardController(DashBoardService dashBoardService)
    {
        _dashBoardService = dashBoardService;
    }
    /// <summary>
    /// Lấy tổng quan gửi email
    /// </summary>
    /// <returns></returns>
    [HttpPost("GetTotalSendEmail")]
    public async Task<IActionResult> GetTotalSendEmail(DashBoardFilter filter)
    {
        return Ok(await _dashBoardService.GetToTalSendEmail(filter));
    }
    /// <summary>
    /// Lấy tổng quan thiết bị
    /// </summary>
    /// <param name="dateTimeFilter"></param>
    /// <returns></returns>
    [HttpPost("GetToTalDevice")]
    public async Task<IActionResult> GetToTalDevice(DateTime? dateTimeFilter)
    {
        return Ok(await _dashBoardService.GetToTalDevice(dateTimeFilter));
    }
    /// <summary>
    /// Lấy tổng quan thiết bị 1
    /// </summary>
    /// <returns></returns>
    [HttpPost("GetToTalDevice1")]
    public async Task<IActionResult> GetToTalDevice1(DashBoardFilter filter)
    {
        return Ok(await _dashBoardService.GetToTalDevice1(filter));
    }
    /// <summary>
    /// Lấy tổng quan thiết bị theo orgId
    /// </summary>
    /// <param name="orgId"></param>
    /// <returns></returns>
    [HttpGet("GetToTalDeviceOrg")]
    public async Task<IActionResult> GetToTalDeviceOrg(string orgId)
    {
        return Ok(await _dashBoardService.GetToTalDeviceOrg(orgId));
    }
    /// <summary>
    /// Lấy danh sách thiết bị theo filter
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    [HttpPost("GetDeviceForStatus")]
    public async Task<IActionResult> GetDeviceForStatus(DBDeviceFilter filter)
    {
        return Ok(await _dashBoardService.GetDeviceForStatus(filter));
    }
    /// <summary>
    /// Lấy tổng quan đồng bộ khuôn mặt
    /// </summary>
    /// <returns></returns>
    [HttpPost("GetTotalStudentFace")]
    public async Task<IActionResult> GetTotalStudentFace(DashBoardFilter filter)
    {
        return Ok(await _dashBoardService.GetTotalStudentFace(filter));
    }
    /// <summary>
    /// Lấy tổng quan điểm danh
    /// </summary>
    /// <returns></returns>
    [HttpPost("GetTotalStudentAttendance")]
    public async Task<IActionResult> GetTotalStudentAttendance(DashBoardFilter filter)
    {
        return Ok(await _dashBoardService.GetTotalStudentAttendance(filter));
    }

    /// <summary>
    /// Lấy tổng quan trường, lớp, học sinh
    /// </summary>
    /// <returns></returns>
    [HttpPost("GetTotalSchool")]
    public async Task<IActionResult> GetTotalSchool(DashBoardFilter filter)
    {
        return Ok(await _dashBoardService.GetTotalSchool(filter));
    }

    /// <summary>
    /// Lấy dữ liệu báo cáo
    /// </summary>
    /// <returns></returns>
    [HttpPost("DashBoardReport")]
    public async Task<IActionResult> DashBoardReport(DashBoardFilter filter)
    {
        return Ok(await _dashBoardService.DashBoardReport(filter));
    }
}