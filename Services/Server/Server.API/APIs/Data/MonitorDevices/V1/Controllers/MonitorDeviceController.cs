﻿using AMMS.Share.WebApp.Helps;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Server.API.SignalRs;
using Server.Application.CronJobs.Params;
using Server.Application.MasterDatas.A2.MonitorDevices.V1;
using Server.Application.MasterDatas.A2.MonitorDevices.V1.Models;
using Server.Application.MasterDatas.A2.Organizations.V1;
using Server.Core.Entities.A2;
using Share.WebApp.Controllers;
using Shared.Core.SignalRs;

namespace Server.API.APIs.Data.MonitorDevices.V1.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
[Authorize("Bearer")]
[AuthorizeMaster]
public class MonitorDeviceController : AuthBaseAPIController
{
    private readonly MonitorDeviceService _monitorDeviceService;
    private readonly ISignalRClientService _signalRService;
    private readonly IHubContext<AmmsHub> _hubContext;
    public MonitorDeviceController(MonitorDeviceService monitorDeviceService, ISignalRClientService signalRService, IHubContext<AmmsHub> hubContext)
    {
        _monitorDeviceService = monitorDeviceService;
        _signalRService = signalRService;
        _hubContext = hubContext;
    }

    /// <summary>
    /// Lấy danh sách theo filter
    /// </summary>
    /// <returns></returns>
    [HttpPost("GetsFilter")]
    public async Task<IActionResult> GetsFilter(MDeviceFilter filter)
    {
        var data = await _monitorDeviceService.GetsFilter(filter);
        return Ok(data);
    }

    /// <summary>
    /// Lấy danh sách tất cả
    /// </summary>
    /// <returns></returns>
    [HttpGet("Gets")]
    public async Task<IActionResult> Gets()
    {
        var data = await _monitorDeviceService.Gets();
        return Ok(data);
    }
    /// <summary>
    /// Cập nhật trạng thái thiết bị
    /// </summary>
    /// <returns></returns>
    [HttpPost("UpdateStatusConnect")]
    public async Task<IActionResult> UpdateStatusConnect(List<MDeviceStatusRequest> requests)
    {
        var data = await _monitorDeviceService.UpdateStatusConnect(requests);
        return Ok(data);
    }

    //[HttpGet("Test")]
    //public async Task<IActionResult> Test()
    //{
    //    Random random = new Random();
    //    int randomNumber = random.Next(1, 5);
    //    var device = new { serialNumber = "test", connectionStatus = true, connectUpdateTime = DateTime.Now };
    //    switch (randomNumber)
    //    {
    //        case 2:
    //            device = new { serialNumber = "test", connectionStatus = false, connectUpdateTime = DateTime.Now };
    //            break;
    //        case 3:
    //            device = new { serialNumber = "test1", connectionStatus = false, connectUpdateTime = DateTime.Now };
    //            break;
    //        case 4:
    //            device = new { serialNumber = "test1", connectionStatus = true, connectUpdateTime = DateTime.Now };
    //            break;
    //    }

    //    //await _hubContext.Clients.All.SendAsync("RefreshDevice", "RefreshDevice", "", "Check kết nối");
    //    if (_signalRService != null && _signalRService.Connection != null && _signalRService.Connection.State == Microsoft.AspNetCore.SignalR.Client.HubConnectionState.Connected)
    //    {
    //        await _signalRService.Connection.InvokeAsync("RefreshDevice", JsonConvert.SerializeObject(new List<object> { device }));
    //    }
    //    return Ok(device);
    //}

    [HttpPost("ChangeStatusDevice")]
    public async Task<IActionResult> ChangeStatusDevice(List<MDeviceStatusRequest> requests)
    {
        if (_signalRService != null && _signalRService.Connection != null && _signalRService.Connection.State == Microsoft.AspNetCore.SignalR.Client.HubConnectionState.Connected)
        {
            await _signalRService.Connection.InvokeAsync("RefreshDevice", JsonConvert.SerializeObject(requests));
        }
        DeviceParam.DataReception = true;
        return Ok(await _monitorDeviceService.UpdateStatusConnect(requests));
    }

    [HttpGet("ChangeStatusDataReception")]
    public async Task<IActionResult> ChangeStatusDataReception()
    {
        DeviceParam.DataReception = !DeviceParam.DataReception;
        return Ok(DeviceParam.DataReception);
    }
}   