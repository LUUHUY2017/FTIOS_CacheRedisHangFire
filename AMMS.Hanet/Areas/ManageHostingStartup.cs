using AMMS.Hanet.Areas;

[assembly: HostingStartup(typeof(ManageHostingStartup))]
namespace AMMS.Hanet.Areas;

public class ManageHostingStartup : IHostingStartup
{
    public void Configure(IWebHostBuilder builder)
    {
        builder.ConfigureServices((context, services) =>
        {
        });
    }
}