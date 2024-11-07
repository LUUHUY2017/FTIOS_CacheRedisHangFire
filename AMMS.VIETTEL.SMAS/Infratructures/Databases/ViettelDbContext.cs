using AMMS.VIETTEL.SMAS.Cores.Entities.A0;
using AMMS.VIETTEL.SMAS.Cores.Entities.A2;
using AMMS.VIETTEL.SMAS.Infratructures.Databases.Seeds;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Core.Datas;

namespace AMMS.VIETTEL.SMAS.Infratructures.Databases;


public class ViettelDbContext : BaseDbContext, IViettelDbContext
{
    // This constructor is used of runit testing
    public ViettelDbContext() : base()
    {
    }
    public ViettelDbContext(DbContextOptions<ViettelDbContext> options, IMediator mediator) : base(options, mediator)
    {
    }

    public DbSet<A0_AttendanceConfig> app_config { get; set; }
    public DbSet<A0_TimeConfig> TimeConfig { get; set; }
    public DbSet<A2_Organization> Organization { get; set; }



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
