using AMMS.Hanet.Applications.MonitorDevices.V1;
using AMMS.Hanet.Applications.MonitorDevices.V1.Models;
using Microsoft.AspNetCore.Mvc;
using Shared.Core.Commons;

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
    /// Lấy danh sách theo filter
    /// </summary>
    /// <returns></returns>
    [HttpPost("GetsFilter")]
    public async Task<IActionResult> GetsFilter(MDeviceFilter filter)
    {
        return Ok(await _monitorDeviceService.Gets(filter));
    }

    /// <summary>
    /// Xóa 
    /// </summary>
    /// <returns></returns>
    [HttpPost("Delete")]
    public async Task<IActionResult> Delete(DeleteRequest request)
    {
        return Ok(await _monitorDeviceService.Delete(request));
    }
}
