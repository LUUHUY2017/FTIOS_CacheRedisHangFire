using AMMS.Share.WebApp.Helps;
using Microsoft.AspNetCore.Mvc;
using Server.Application.MasterDatas.A2.DashBoards.V1;
using Share.WebApp.Controllers;

namespace Server.API.APIs.Data.DashBoards.V1.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[AuthorizeMaster]
public class DashBoardController : AuthBaseAPIController
{
    private readonly DashBoardService _dashBoardService;

    public DashBoardController(DashBoardService dashBoardService)
    {
        _dashBoardService = dashBoardService;
    }

    [HttpPost("GetTotalSendEmail")]
    public async Task<IActionResult> GetTotalSendEmail(DateTime? dateTimeFilter)
    {
        return Ok(await _dashBoardService.GetToTalSendEmail(dateTimeFilter));
    }
}