using AMMS.VIETTEL.SMAS.Cores.Entities.A0;
using AMMS.VIETTEL.SMAS.Cores.Entities.A2;
using AMMS.VIETTEL.SMAS.Cores.Entities.TA;
using Microsoft.EntityFrameworkCore;

namespace AMMS.VIETTEL.SMAS.Infratructures.Databases;

public interface IViettelDbContext
{
    public DbSet<AttendanceConfig> AttendanceConfig { get; set; }
    public DbSet<TimeConfig> TimeConfig { get; set; }
    public DbSet<Organization> Organization { get; set; }

    //
    public DbSet<ScheduleSendMail> ScheduleSendMail { get; set; }
    public DbSet<ScheduleSendMailDetail> ScheduleSendMailDetail { get; set; }
    public DbSet<SendEmails> SendEmail { get; set; }
    public DbSet<SendEmailLogs> SendEmailLogs { get; set; }
    public DbSet<EmailConfiguration> EmailConfiguration { get; set; }   
    public DbSet<Person> Person { get; set; }
    public DbSet<PersonType> PersonType { get; set; }
    public DbSet<Student> Student { get; set; }
    public DbSet<PersonFace> PersonFace { get; set; }
    public DbSet<PersonSynToDevice> PersonSynToDevice { get; set; }
    public DbSet<Device> Device { get; set; }
    public DbSet<ScheduleJob> ScheduleJob { get; set; }
    public DbSet<ScheduleJobLog> ScheduleJobLog { get; set; }

    #region TA
    public DbSet<TimeAttendenceEvent> TimeAttendenceEvent { get; set; }
    public DbSet<TimeAttendenceDetail> TimeAttendenceDetail { get; set; }
    public DbSet<TimeAttendenceSync> TimeAttendenceSync { get; set; }
    #endregion
    Task<int> SaveChangesAsync();
}
