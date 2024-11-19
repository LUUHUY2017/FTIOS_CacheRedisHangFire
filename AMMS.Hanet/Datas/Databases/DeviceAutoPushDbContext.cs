using AMMS.Hanet.Datas.Databases.Seeds;
using AMMS.Hanet.Datas.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Core.Datas;

namespace AMMS.Hanet.Datas.Databases;


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
    public DbSet<hanet_app_config> app_config { get; set; }
    public DbSet<hanet_commandlog> hanet_commandlog { get; set; }
    public DbSet<hanet_transaction> hanet_transaction { get; set; }



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("Hanet"); //Tạo Schema

        //modelBuilder.Entity<ApplicationUser>(entity =>
        //{
        //    entity.ToTable(name: "User");
        //}); //Tạo bảng

        modelBuilder.Seed();
    }
}
