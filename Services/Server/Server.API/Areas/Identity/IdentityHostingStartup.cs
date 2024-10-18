using AMMS.WebAPI.Areas.Identity;

[assembly: HostingStartup(typeof(IdentityHostingStartup))]
namespace AMMS.WebAPI.Areas.Identity;

public class IdentityHostingStartup : IHostingStartup
{
    public void Configure(IWebHostBuilder builder)
    {
        builder.ConfigureServices((context, services) =>
        {
        });
    }
}