using AMMS.VIETTEL.SMAS.Cores.Entities;
using AMMS.VIETTEL.SMAS.Infratructures.Databases.Seeds;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Core.Datas;

namespace AMMS.VIETTEL.SMAS.Infratructures.Databases;


public class DeviceAutoPushDbContext : BaseDbContext, IDeviceAutoPushDbContext
{
    // This constructor is used of runit testing
    public DeviceAutoPushDbContext() : base()
    {
    }
    public DeviceAutoPushDbContext(DbContextOptions<DeviceAutoPushDbContext> options, IMediator mediator) : base(options, mediator)
    {
    }

    public DbSet<hanet_terminal> hanet_terminal { get; set; }
    public DbSet<app_config> app_config { get; set; }
    public DbSet<hanet_commandlog> hanet_commandlog { get; set; }
    public DbSet<hanet_transaction> hanet_transaction { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("SMAS"); //Tạo Schema

        //modelBuilder.Entity<ApplicationUser>(entity =>
        //{
        //    entity.ToTable(name: "User");
        //}); //Tạo bảng

        modelBuilder.Seed();
    }
}
