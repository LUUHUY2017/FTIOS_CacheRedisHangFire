using Microsoft.AspNetCore.Mvc;

namespace AMMS.VIETTEL.SMAS.APIControllers.AppConfigs.V1.AppConfigControllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]

public class AppConfigController : ControllerBase
{
    [HttpGet("Test")]
    public async Task<IActionResult> Test()
    {
        return Ok();
    }
}