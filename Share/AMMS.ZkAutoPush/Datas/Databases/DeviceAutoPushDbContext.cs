using AMMS.ZkAutoPush.Datas.Databases.Seeds;
using AMMS.ZkAutoPush.Datas.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Core.Datas;

namespace AMMS.ZkAutoPush.Datas.Databases;


public class DeviceAutoPushDbContext : BaseDbContext, IDeviceAutoPushDbContext
{
    // This constructor is used of runit testing
    public DeviceAutoPushDbContext() : base()
    {
    }
    public DeviceAutoPushDbContext(DbContextOptions<DeviceAutoPushDbContext> options, IMediator mediator) : base(options, mediator)
    {
    }

    public DbSet<zk_user> zk_user { get; set; }
    public DbSet<zk_terminalcommandlog> zk_terminalcommandlog { get; set; }
    public DbSet<zk_biodata> zk_biodata { get; set; }
    public DbSet<zk_biophoto> zk_biophoto { get; set; }
    public DbSet<zk_terminal> zk_terminal { get; set; }
    public DbSet<zk_transaction> zk_transaction { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("Zkteco"); //Tạo Schema

        //modelBuilder.Entity<ApplicationUser>(entity =>
        //{
        //    entity.ToTable(name: "User");
        //}); //Tạo bảng

        modelBuilder.Seed();
    }
}
