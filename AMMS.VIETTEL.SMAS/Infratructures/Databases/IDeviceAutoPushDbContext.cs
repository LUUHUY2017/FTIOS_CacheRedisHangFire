using AMMS.VIETTEL.SMAS.Cores.Entities;
using Microsoft.EntityFrameworkCore;

namespace AMMS.VIETTEL.SMAS.Infratructures.Databases;

public interface IDeviceAutoPushDbContext
{
    public DbSet<hanet_terminal> hanet_terminal { get; set; }
    public DbSet<app_config> app_config { get; set; }
    public DbSet<hanet_commandlog> hanet_commandlog { get; set; }
    public DbSet<hanet_transaction> hanet_transaction { get; set; }

    Task<int> SaveChangesAsync();
}
