using AMMS.DeviceData.RabbitMq;
using AMMS.Hanet.Applications.V1.Service;
using AMMS.Hanet.Data;
using AMMS.Hanet.Data.Response;
using EventBus.Messages;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Share.WebApp.Controllers;
using Share.WebApp.Helps;
using System.Text;

namespace AMMS.Hanet.APIControllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
//[AuthorizeClientAPI(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleConst.AdminPage)]
//[AuthorizeClientAPI(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//public class DeviceController : AuthBaseAPIController
public class DeviceController : ControllerBase
{
    HANET_Server_Push_Service _HANET_Process_Service;
    private readonly IEventBusAdapter _eventBusAdapter;
    private readonly IConfiguration _configuration;

    public DeviceController(HANET_Server_Push_Service HANET_Process_Service, IConfiguration configuration,EventBusAdapter eventBusAdapter)
    {
        _HANET_Process_Service = HANET_Process_Service;
        _eventBusAdapter = eventBusAdapter;
        _configuration = configuration;
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
            PersonCode = "333",
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

            var aa = await _eventBusAdapter.GetSendEndpointAsync(_configuration.GetValue<string>("DataArea") + EventBusConstants.Hanet_Device_Push_D2S);
            await aa.Send(data);

        }
        return Ok();
    }
}
