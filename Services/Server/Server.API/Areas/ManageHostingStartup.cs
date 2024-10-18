using Server.API.Areas;

[assembly: HostingStartup(typeof(ManageHostingStartup))]
namespace Server.API.Areas;

public class ManageHostingStartup : IHostingStartup
{
    public void Configure(IWebHostBuilder builder)
    {
        builder.ConfigureServices((context, services) =>
        {
        });
    }
}