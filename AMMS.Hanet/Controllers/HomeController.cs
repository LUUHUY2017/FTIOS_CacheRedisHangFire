using AMMS.Hanet.Applications.AppConfigs.V1;
using Microsoft.AspNetCore.Mvc;
using Share.WebApp.Controllers;
using Share.WebApp.Helps;

namespace AMMS.Hanet.Controllers;

[AuthorizeClient]
public class HomeController : AuthBaseController
{
    private readonly AppConfigService _appConfigService;
    public HomeController(AppConfigService appConfigService)
    {
        _appConfigService = appConfigService;
    }
    public async Task<IActionResult> Index()
    {
        await _appConfigService.GetFirstOrDefault();
        return View();
    }
}
