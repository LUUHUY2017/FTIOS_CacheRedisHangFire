using AMMS.VIETTEL.SMAS.Cores.Entities.A0;
using AMMS.VIETTEL.SMAS.Cores.Entities.A2;
using AMMS.VIETTEL.SMAS.Cores.Entities.TA;
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

    public DbSet<AttendanceConfig> AttendanceConfig { get; set; }
    public DbSet<TimeConfig> TimeConfig { get; set; }
    public DbSet<Organization> Organization { get; set; }
    public DbSet<Device> Device { get; set; }

    //
    public DbSet<ScheduleSendMail> ScheduleSendMail { get; set; }
    public DbSet<ScheduleSendMailDetail> ScheduleSendMailDetail { get; set; }
    public DbSet<SendEmails> SendEmail { get; set; }
    public DbSet<SendEmailLogs> SendEmailLogs { get; set; }
    public DbSet<EmailConfiguration> EmailConfiguration { get; set; }

    public DbSet<Person> Person { get; set; }
    public DbSet<Student> Student { get; set; }
    public DbSet<PersonFace> PersonFace { get; set; }
    public DbSet<PersonSynToDevice> PersonSynToDevice { get; set; }


    public DbSet<ScheduleJob> ScheduleJob { get; set; }
    public DbSet<ScheduleJobLog> ScheduleJobLog { get; set; }

    #region TA
    public DbSet<TimeAttendenceEvent> TimeAttendenceEvent { get; set; }
    public DbSet<TimeAttendenceSync> TimeAttendenceSync { get; set; }


    public DbSet<SchoolYear> SchoolYear { get; set; }
    public DbSet<ClassRoom> ClassRoom { get; set; }
    public DbSet<StudentClassRoomYear> StudentClassRoomYear { get; set; }



    #endregion
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
