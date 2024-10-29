using AMMS.ZkAutoPush.Areas;

[assembly: HostingStartup(typeof(ManageHostingStartup))]
namespace AMMS.ZkAutoPush.Areas;

public class ManageHostingStartup : IHostingStartup
{
    public void Configure(IWebHostBuilder builder)
    {
        builder.ConfigureServices((context, services) =>
        {
        });
    }
}