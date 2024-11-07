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
    public DbSet<A0_EmailConfiguration> A0_EmailConfiguration { get; set; }
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
    public DbSet<A2_Person> A2_Person { get; set; }
    public DbSet<A2_PersonType> A2_PersonType { get; set; }
    public DbSet<A2_Student> A2_Student { get; set; }
    public DbSet<A2_PersonFace> A2_PersonFace { get; set; }
    public DbSet<A2_PersonSynToDevice> A2_PersonSynToDevice { get; set; }
    public DbSet<ScheduleJob> A2_ScheduleJob { get; set; }
    public DbSet<ScheduleJobLog> A2_ScheduleJobLog { get; set; }

    #endregion

    #region A3
    public DbSet<A3_Image> A3_Image { get; set; }
    #endregion

    #region GIO
    public DbSet<GIO_VehicleInOut> GIO_VehicleInOut { get; set; }

    #endregion

    #region TA
    public DbSet<TA_TimeAttendenceEvent> TA_TimeAttendenceEvent { get; set; }
    public DbSet<TA_TimeAttendenceDetail> TA_TimeAttendenceDetail { get; set; }
    public DbSet<TA_TimeAttendenceSync> TA_TimeAttendenceSync { get; set; }
    #endregion

    Task<int> SaveChangesAsync();
}
