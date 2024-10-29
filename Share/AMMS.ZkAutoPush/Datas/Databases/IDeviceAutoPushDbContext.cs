using AMMS.ZkAutoPush.Datas.Entities;
using Microsoft.EntityFrameworkCore;

namespace AMMS.ZkAutoPush.Datas.Databases;

public interface IDeviceAutoPushDbContext
{
    public DbSet<zk_user> zk_user { get; set; }
    public DbSet<zk_terminalcommandlog> zk_terminalcommandlog { get; set; }
    public DbSet<zk_biodata> zk_biodata { get; set; }
    public DbSet<zk_biophoto> zk_biophoto { get; set; }
    public DbSet<zk_terminal> zk_terminal { get; set; }
    public DbSet<zk_transaction> zk_transaction { get; set; }


    Task<int> SaveChangesAsync();
}
