﻿using AMMS.DeviceData.RabbitMq;
using AMMS.Hanet.Applications.V1.Service;
using AMMS.Hanet.Data;
using AMMS.Hanet.Data.Response;
using EventBus.Messages;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace AMMS.Hanet.APIControllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class DeviceController : ControllerBase
{
    HANET_Server_Push_Service _HANET_Process_Service;
    HANET_Device_Reponse_Service _HANET_Device_Reponse_Service;
    HANET_API_Service _hANET_API_Service;
    private readonly IEventBusAdapter _eventBusAdapter;
    private readonly IConfiguration _configuration;

    public DeviceController(HANET_Server_Push_Service HANET_Process_Service, IConfiguration configuration, IEventBusAdapter eventBusAdapter, HANET_API_Service hANET_API_Service, HANET_Device_Reponse_Service hANET_Device_Reponse_Service)
    {
        _HANET_Process_Service = HANET_Process_Service;
        _eventBusAdapter = eventBusAdapter;
        _configuration = configuration;
        _hANET_API_Service = hANET_API_Service;
        _HANET_Device_Reponse_Service = hANET_Device_Reponse_Service;
    }

    /// <summary>
    /// Lấy danh sách các thiết bị của user. Thông tin response bao gồm các thông tin cơ bản là: deviceID, deviceName, placeName, address.
    /// </summary>
    /// <returns></returns>
    [HttpPost("getListDevice")]
    public async Task<IActionResult> GetListDeviceAsync()
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "https://partner.hanet.ai/device/getListDevice");
        var collection = new List<KeyValuePair<string, string>>();
        collection.Add(new("token", HanetParam.Token.access_token));
        var content = new FormUrlEncodedContent(collection);
        request.Content = content;
        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        Console.WriteLine(await response.Content.ReadAsStringAsync());

        return Ok();
    }
    /// <summary>
    /// Danh sách vị trí
    /// </summary>
    /// <returns></returns>
    [HttpPost("getListPlace")]
    public async Task<IActionResult> GetListPlaceAsync()
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "https://partner.hanet.ai/place/getPlaces");
        var collection = new List<KeyValuePair<string, string>>();
        collection.Add(new("token", HanetParam.Token.access_token));
        var content = new FormUrlEncodedContent(collection);
        request.Content = content;
        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        Console.WriteLine(await response.Content.ReadAsStringAsync());

        return Ok();
    }
    /// <summary>
    /// Test api hanet
    /// </summary>
    /// <returns></returns>
    [HttpPost("testAPI")]
    public async Task<IActionResult> TestAPI()
    {
        TA_PersonInfo user = new TA_PersonInfo()
        {
            PersonCode = "47916397",
            FullName = @"Lê Hoàng Bảo",

        };

        RB_ServerRequest request = new RB_ServerRequest
        {
            Action = ServerRequestAction.ActionAdd,
            RequestType = ServerRequestType.UserInfo,
            Id = Guid.NewGuid().ToString(),
            DeviceId = Guid.NewGuid().ToString(),
            DeviceModel = "Hanet",
            RequestParam = JsonConvert.SerializeObject(user)
        };


        await _HANET_Process_Service.ProcessDataServerPush(request);

        return Ok();
    }
    /// <summary>
    /// Api đẩy dữ liệu của thiết bị vào
    /// </summary>
    /// <returns></returns>
    [HttpPost("PushData")]
    public async Task<IActionResult> PushDataController()
    {
        using (StreamReader reader = new StreamReader(HttpContext.Request.Body, Encoding.UTF8))
        {
            var body = await reader.ReadToEndAsync();

            if (string.IsNullOrEmpty(body))
            {
                return Ok();
            }
            Hanet_Device_Data data = new Hanet_Device_Data()
            {
                data = body
            };

            var aa = await _eventBusAdapter.GetSendEndpointAsync($"{_configuration["DataArea"]}_{EventBusConstants.HANET}{EventBusConstants.Hanet_Device_Push_D2S}");
            await aa.Send(data);

        }
        return Ok();
    }
    /// <summary>
    /// Lấy dữ liệu khuôn mặt
    /// </summary>
    /// <returns></returns>
    [HttpPost("getAllUserFace")]
    public async Task<IActionResult> GetAllUserFace()
    {
        //var count = await _hANET_API_Service.GetCountUserByPlace(HanetParam.PlaceId);

        //Console.WriteLine(count.ToString());
        var countData = 1;
        var totalData = 0;
        int page = 0;
        while (countData > 0)
        {
            page++;
            var top100Data = await _hANET_API_Service.UserByPlaceId(page);
            foreach (var item in top100Data)
            {
                if (item.departmentID != "8235")
                    await _HANET_Device_Reponse_Service.AddUserData(item);
            }
            countData = top100Data.Count;
            totalData += countData;
        }




        return Ok(totalData);
    }
    /// <summary>
    /// Lấy dữ liệu chấm công theo thời gian
    /// </summary>
    /// <returns></returns>
    [HttpGet("getCheckInByTime")]
    public async Task<IActionResult> GetAllUserFace(DateTime startDate, DateTime endDate)
    {
        int page = 0;

        var top100Data = await _hANET_API_Service.GetCheckinByTime(startDate, endDate, page, 50);

        foreach (var item in top100Data)
        {
            var data = await _HANET_Device_Reponse_Service.AddTransactionHistoryLog(item);

            if (data != null && item != null)
            {
                double ticks = item.checkinTime.Value;
                TimeSpan time = TimeSpan.FromMilliseconds(ticks);
                DateTime date = new DateTime(1970, 1, 1) + time;

                TA_AttendenceHistory tA_AttendenceHistory = new TA_AttendenceHistory()
                {
                    Id = data.id,
                    DeviceId = data.deviceID,
                    PersonCode = item.aliasID,
                    //PersonId = taData.PersonId,
                    SerialNumber = item.deviceName,
                    TimeEvent = date,
                    Type = (int)TA_AttendenceType.Face,
                };

                RB_DataResponse rB_Response = new RB_DataResponse()
                {
                    Id = tA_AttendenceHistory.Id,
                    Content = JsonConvert.SerializeObject(tA_AttendenceHistory),
                    ReponseType = RB_DataResponseType.AttendenceHistory,
                };

                var aa = await _eventBusAdapter.GetSendEndpointAsync($"{_configuration["DataArea"]}{EventBusConstants.Data_Auto_Push_D2S}");

                await aa.Send(rB_Response);

            }

        }

        return Ok(top100Data);
    }

}
