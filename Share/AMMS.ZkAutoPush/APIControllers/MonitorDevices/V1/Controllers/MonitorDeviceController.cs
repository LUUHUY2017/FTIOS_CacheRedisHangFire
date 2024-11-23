using AMMS.ZkAutoPush.Applications.MonitorDevices.V1.Models;
using Microsoft.AspNetCore.Mvc;
using Share.WebApp.Helps;
using Shared.Core.Commons;

namespace AMMS.ZkAutoPush.APIControllers.MonitorDevices.V1.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[AuthorizeClient]

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

    /// <summary>
    /// Reboot 
    /// </summary>
    /// <returns></returns>
    [HttpPost("Reboot")]
    public async Task<IActionResult> Reboot(ObjectString request)
    {
        return Ok(await _monitorDeviceService.Reboot(request));
    }

    /// <summary>
    /// Xoá hết dữ liệu 
    /// </summary>
    /// <returns></returns>
    [HttpPost("DeleteAll")]
    public async Task<IActionResult> DeleteAll(ObjectString request)
    {
        return Ok(await _monitorDeviceService.DeleteAllData(request));
    }

    /// <summary>
    /// Xoá hết dữ liệu chấm công 
    /// </summary>
    /// <returns></returns>
    [HttpPost("DeleteAllLog")]
    public async Task<IActionResult> DeleteAllLog(ObjectString request)
    {
        return Ok(await _monitorDeviceService.DeleteAllLog(request));
    }

}
