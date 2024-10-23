using AMMS.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Server.Core.Entities.A0;
using Server.Core.Entities.A2;
using Server.Core.Entities.A3;
using Server.Core.Entities.GIO;

namespace Server.Infrastructure.Datas.MasterData;

public class MasterDataDbContext : BaseDbContext, IMasterDataDbContext
{
    // This constructor is used of runit testing
    public MasterDataDbContext() : base()
    { }

    public MasterDataDbContext(DbContextOptions<MasterDataDbContext> options, IMediator mediator) : base(options, mediator)
    { }

    #region A0
    public DbSet<A0_EmailConfiguration> A0_EmailConfiguration { get; set; }
    public DbSet<A0_PersonType> A0_PersonType { get; set; }
    public DbSet<A0_Person> A0_Person { get; set; }

    public DbSet<A0_RoleGroup> A0_RoleGroup { get; set; }
    public DbSet<A0_RoleGroupDetail> A0_RoleGroupDetail { get; set; }
    public DbSet<A0_RoleGroupUser> A0_RoleGroupUser { get; set; }
    public DbSet<A0_Page> A0_Page { get; set; }
    public DbSet<A0_RoleGroupPage> A0_RoleGroupPage { get; set; }
    public DbSet<A0_AttendanceConfig> A0_AttendanceConfig { get; set; }
    public DbSet<A0_TimeConfig> A0_TimeConfig { get; set; }
    #endregion


    #region A1
    #endregion

    #region A2
    public DbSet<A2_ScheduleSendMail> A2_ScheduleSendMail { get; set; }
    public DbSet<A2_ScheduleSendMailDetail> A2_ScheduleSendMailDetail { get; set; }
    public DbSet<A2_SendEmail> A2_SendEmail { get; set; }
    public DbSet<A2_SendEmailLog> A2_SendEmailLog { get; set; }
    public DbSet<A2_Device> A2_Device { get; set; }
    public DbSet<A2_Organization> A2_Organization { get; set; }
    public DbSet<A2_BusinessUnit> A2_BusinessUnit { get; set; }
    public DbSet<A2_Notification> A2_Notification { get; set; }
    public DbSet<A2_Lane> A2_Lane { get; set; }
    #endregion

    #region
    public DbSet<A3_Image> A3_Image { get; set; }
    #endregion

    #region GIO
    public DbSet<GIO_VehicleInOut> GIO_VehicleInOut { get; set; }

    #endregion



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //modelBuilder.HasDefaultSchema("Identity"); //Tạo Schema

        //modelBuilder.Entity<ApplicationUser>(entity =>
        //{
        //    entity.ToTable(name: "User");
        //}); //Tạo bảng

        //modelBuilder.Seed();
    }
}
