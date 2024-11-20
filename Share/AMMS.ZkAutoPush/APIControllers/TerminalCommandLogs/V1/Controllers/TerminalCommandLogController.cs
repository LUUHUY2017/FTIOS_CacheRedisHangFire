using AMMS.DeviceData.RabbitMq;
using AMMS.ZkAutoPush.Applications;
using AMMS.ZkAutoPush.Applications.TerminalCommandLogs.V1;
using AMMS.ZkAutoPush.Applications.TerminalCommandLogs.V1.Models;
using AMMS.ZkAutoPush.Applications.V1;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Operation.Overlay.Snap;
using Newtonsoft.Json;
using Share.WebApp.Helps;
using Shared.Core.Commons;
using Shared.Core.Loggers;

namespace AMMS.ZkAutoPush.APIControllers.TerminalCommandLogs.V1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[AuthorizeClient]

public class TerminalCommandLogController : ControllerBase
{
    private readonly TerminalCommandLogService _terminalCommandLogService;
    private readonly DeviceCommandCacheService _deviceCommandCacheService;
    private readonly ZK_SV_PUSHService _zK_SV_PUSHService;
    public TerminalCommandLogController(TerminalCommandLogService terminalCommandLogService, DeviceCommandCacheService deviceCommandCacheService, ZK_SV_PUSHService zK_SV_PUSHService)
    {
        _terminalCommandLogService = terminalCommandLogService;
        _deviceCommandCacheService = deviceCommandCacheService;
        _zK_SV_PUSHService = zK_SV_PUSHService;
    }

    [HttpGet("GetDetail")]
    public async Task<IActionResult> GetDetail(string id)
    {
        return Ok(await _terminalCommandLogService.GetDetail(id));
    }

    [HttpPost("GetsFilter")]
    public async Task<IActionResult> GetsFilter(TerminalCommandLogFilter filter)
    {
        return Ok(await _terminalCommandLogService.Gets(filter));
    }

    [HttpPost("Delete")]
    public async Task<IActionResult> Delete(DeleteRequest request)
    {
        return Ok(await _terminalCommandLogService.Delete(request));
    }
    [HttpPost("ReSend")]
    public async Task<IActionResult> ReSend(DeleteRequest request)
    {
        return Ok(await _terminalCommandLogService.Resend(request));
    }
    [HttpGet("GetDataByTime")]
    public async Task<IActionResult> GetDataByTime(DateTime startDate, DateTime endDate, string sn = "PYA8241500003")
    {
        try
        {
            RB_ServerRequest request = new RB_ServerRequest()
            {
                SerialNumber = sn,
                Id = Guid.NewGuid().ToString(),
                Action = ServerRequestAction.ActionGetData,
                RequestType = ServerRequestType.TAData,
                DeviceModel = EventBusConstants.ZKTECO,
                RequestId = DateTime.Now.TimeOfDay.TotalMilliseconds,
                DeviceId = Guid.NewGuid().ToString(),

            };
            TA_AttendenceHistoryRequest rq = new TA_AttendenceHistoryRequest()
            {
                StartDate = startDate,
                EndDate = endDate,
            };

            request.RequestParam = JsonConvert.SerializeObject(rq);
            await _zK_SV_PUSHService.Process(request);
        }
        catch (Exception ex)
        {

            Logger.Error(ex);
        }

        return Ok();
    }
}
