using AMMS.Hanet.Applications.AppConfigs.V1;
using Microsoft.AspNetCore.Mvc;

namespace AMMS.Hanet.Controllers;


public class HomeController : Controller
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
