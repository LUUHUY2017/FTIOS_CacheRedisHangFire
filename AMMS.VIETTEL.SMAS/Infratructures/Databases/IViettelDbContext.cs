using AMMS.VIETTEL.SMAS.Cores.Entities.A0;
using AMMS.VIETTEL.SMAS.Cores.Entities.A2;
using Microsoft.EntityFrameworkCore;

namespace AMMS.VIETTEL.SMAS.Infratructures.Databases;

public interface IViettelDbContext
{
    public DbSet<A0_AttendanceConfig> app_config { get; set; }
    public DbSet<A0_TimeConfig> TimeConfig { get; set; }
    public DbSet<A2_Organization> Organization { get; set; }

    Task<int> SaveChangesAsync();
}
