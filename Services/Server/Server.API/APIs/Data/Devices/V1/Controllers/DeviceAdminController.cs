using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Application.MasterDatas.A2.Devices.Models;
using Server.Application.MasterDatas.A2.Devices;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.Devices.RequeResponsessts;
using Server.Core.Interfaces.A2.Devices.Requests;
using Share.Core.Pagination;
using Share.WebApp.Controllers;
using Shared.Core.Commons;
using AMMS.Share.WebApp.Helps;

namespace Server.API.APIs.Data.Devices.V1.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize("Bearer")]
[AuthorizeMaster]
public class DeviceAdminController : AuthBaseAPIController
{
    private readonly DeviceAdminService _deviceAdminService;
    private readonly IMapper _mapper;
    private readonly IUriService _uriService;

    public DeviceAdminController(DeviceAdminService deviceAdminService, IMapper mapper, IUriService uriService)
    {
        _deviceAdminService = deviceAdminService;
        _mapper = mapper;
        _uriService = uriService;
    }

    /// <summary>
    /// Lấy danh sách thiết bị
    /// </summary>
    /// <returns></returns>
    [HttpPost("GetDevices")]
    public async Task<IActionResult> GetDevices(DeviceFilterRequest filter)
    {
        return Ok(await _deviceAdminService.GetDevices(filter));
    }

    /// <summary>
    /// Lấy danh sách đang hoạt động
    /// </summary>
    /// <returns></returns>
    [HttpGet("GetDeviceSelect")]
    public async Task<IActionResult> GetDeviceSelect()
    {
        return Ok(await _deviceAdminService.GetDeviceSelect());
    }

    /// <summary>
    /// Cập nhật thiết bị
    /// </summary>
    /// <returns></returns>
    [HttpPost("Update")]
    public async Task<IActionResult> Update(DeviceRequest request)
    {
        return Ok(await _deviceAdminService.Update(request));
    }


    /// <summary>
    /// Bỏ chọn thiết bị
    /// </summary>
    /// <returns></returns>
    [HttpPost("UnSelected")]
    public async Task<IActionResult> UnSelected(DeviceRequest request)
    {
        return Ok(await _deviceAdminService.UnSelected(request));
    }

    /// <summary>
    /// Tắt hoạt động 
    /// </summary>
    /// <returns></returns>
    [HttpPost("InActive")]
    public async Task<IActionResult> Active(ActiveRequest request)
    {
        return Ok(await _deviceAdminService.Active(request));
    }

    /// <summary>
    /// Bật hoạt động 
    /// </summary>
    /// <returns></returns>
    [HttpPost("Active")]
    public async Task<IActionResult> InActive(InactiveRequest request)
    {
        return Ok(await _deviceAdminService.InActive(request));
    }
}