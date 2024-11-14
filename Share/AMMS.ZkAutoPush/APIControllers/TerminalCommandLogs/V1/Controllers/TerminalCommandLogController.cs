﻿using AMMS.ZkAutoPush.Applications.TerminalCommandLogs.V1;
using AMMS.ZkAutoPush.Applications.TerminalCommandLogs.V1.Models;
using Microsoft.AspNetCore.Mvc;
using Shared.Core.Commons;

namespace AMMS.ZkAutoPush.APIControllers.TerminalCommandLogs.V1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class TerminalCommandLogController : ControllerBase
{
    private readonly TerminalCommandLogService _terminalCommandLogService;
    public TerminalCommandLogController(TerminalCommandLogService terminalCommandLogService)
    {
        _terminalCommandLogService = terminalCommandLogService;
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
}