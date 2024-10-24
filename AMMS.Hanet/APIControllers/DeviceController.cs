using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Share.WebApp.Controllers;
using Share.WebApp.Helps;

namespace AMMS.Hanet.APIControllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
//[AuthorizeClientAPI(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = RoleConst.AdminPage)]
//[AuthorizeClientAPI(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//public class DeviceController : AuthBaseAPIController
public class DeviceController : ControllerBase
{
    /// <summary>
    /// Lấy danh sách các thiết bị của user. Thông tin response bao gồm các thông tin cơ bản là: deviceID, deviceName, placeName, address.
    /// </summary>
    /// <returns></returns>
    [HttpPost("getListDevice")]
    public IActionResult GetListDevice()
    {
        return Ok();
    }
}
