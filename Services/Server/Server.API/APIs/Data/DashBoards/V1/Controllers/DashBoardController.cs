using AMMS.Share.WebApp.Helps;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Application.MasterDatas.A2.DashBoards.V1;
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

    [HttpGet("GetTotalSendEmail")]
    public async Task<IActionResult> GetTotalSendEmail()
    {
        return Ok(await _dashBoardService.GetToTalSendEmail());
    }

    [HttpPost("GetToTalDevice")]
    public async Task<IActionResult> GetToTalDevice(DateTime? dateTimeFilter)
    {
        return Ok(await _dashBoardService.GetToTalDevice(dateTimeFilter));
    }

    [HttpGet("GetToTalDeviceOrg")]
    public async Task<IActionResult> GetToTalDeviceOrg(string orgId)
    {
        return Ok(await _dashBoardService.GetToTalDeviceOrg(orgId));
    }

    [HttpPost("GetDeviceForStatus")]
    public async Task<IActionResult> GetDeviceForStatus(DBDeviceFilter filter)
    {
        return Ok(await _dashBoardService.GetDeviceForStatus(filter));
    }
}