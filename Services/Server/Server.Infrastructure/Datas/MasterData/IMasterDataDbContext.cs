using AMMS.Notification.Datas;
using AMMS.Notification.Datas.Entities;
using Microsoft.EntityFrameworkCore;
using Server.Core.Entities.A0;
using Server.Core.Entities.A2;
using Server.Core.Entities.A3;
using Server.Core.Entities.GIO;
using Server.Core.Entities.TA;

namespace Server.Infrastructure.Datas.MasterData;

public interface IMasterDataDbContext
{
    #region A0
    public DbSet<EmailConfiguration> EmailConfiguration { get; set; }
    public DbSet<RoleGroup> RoleGroup { get; set; }
    public DbSet<RoleGroupDetail> RoleGroupDetail { get; set; }
    public DbSet<RoleGroupUser> RoleGroupUser { get; set; }
    public DbSet<Pages> Page { get; set; }
    public DbSet<RoleGroupPage> RoleGroupPage { get; set; }
    public DbSet<AttendanceConfig> AttendanceConfig { get; set; }
    public DbSet<TimeConfig> TimeConfig { get; set; }
    public DbSet<AttendanceTimeConfig> AttendanceTimeConfig { get; set; }

    #endregion

    #region A1
    #endregion

    #region A2

    public DbSet<ScheduleSendMail> ScheduleSendMail { get; set; }
    public DbSet<ScheduleSendMailDetail> ScheduleSendMailDetail { get; set; }
    public DbSet<SendEmails> SendEmail { get; set; }
    public DbSet<SendEmailLogs> SendEmailLogs { get; set; }
    public DbSet<Device> Device { get; set; }
    public DbSet<Organization> Organization { get; set; }
    public DbSet<BusinessUnit> BusinessUnit { get; set; }
    public DbSet<Notifications> Notification { get; set; }
    public DbSet<Lane> Lane { get; set; }
    public DbSet<Person> Person { get; set; }
    public DbSet<PersonType> PersonType { get; set; }
    public DbSet<Student> Student { get; set; }
    public DbSet<PersonFace> PersonFace { get; set; }
    public DbSet<PersonSynToDevice> PersonSynToDevice { get; set; }
    public DbSet<ScheduleJob> ScheduleJob { get; set; }
    public DbSet<ScheduleJobLog> ScheduleJobLog { get; set; }


    public DbSet<SchoolYear> SchoolYear { get; set; }
    public DbSet<ClassRoom> ClassRoom { get; set; }
    public DbSet<StudentClassRoomYear> StudentClassRoomYear { get; set; }

    #endregion

    #region A3
    public DbSet<Images> Image { get; set; }
    #endregion

    #region GIO
    public DbSet<VehicleInOut> VehicleInOut { get; set; }

    #endregion

    #region TA
    public DbSet<TimeAttendenceEvent> TimeAttendenceEvent { get; set; }
    public DbSet<TimeAttendenceDetail> TimeAttendenceDetail { get; set; }
    public DbSet<TimeAttendenceSync> TimeAttendenceSync { get; set; }
    #endregion

    Task<int> SaveChangesAsync();
}
