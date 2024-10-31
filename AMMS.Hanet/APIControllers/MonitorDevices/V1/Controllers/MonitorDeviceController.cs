using AMMS.Hanet.Applications.AppConfigs.V1;
using AMMS.Hanet.Applications.MonitorDevices.V1;
using Microsoft.AspNetCore.Mvc;

namespace AMMS.Hanet.APIControllers.MonitorDevices.V1.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]

public class MonitorDeviceController : ControllerBase
{
    private readonly MonitorDeviceService _monitorDeviceService;
    public MonitorDeviceController(MonitorDeviceService monitorDeviceService)
    {
        _monitorDeviceService = monitorDeviceService;
    }

    /// <summary>
    /// Lấy danh sách
    /// </summary>
    /// <returns></returns>
    [HttpGet("Gets")]
    public async Task<IActionResult> Gets()
    {
        return Ok(await _monitorDeviceService.Gets());
    }
}
