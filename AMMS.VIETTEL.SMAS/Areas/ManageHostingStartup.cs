using AMMS.VIETTEL.SMAS.Areas;

[assembly: HostingStartup(typeof(ManageHostingStartup))]
namespace AMMS.VIETTEL.SMAS.Areas;

public class ManageHostingStartup : IHostingStartup
{
    public void Configure(IWebHostBuilder builder)
    {
        builder.ConfigureServices((context, services) =>
        {
        });
    }
}