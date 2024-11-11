using Server.Core.Entities.TA;

namespace Server.Core.Interfaces.TimeAttendenceSyncs.Responses;

public class AttendenceSyncReportRes : TimeAttendenceSync
{
    public string StudentCode { get; set; }
    public string StudentName { get; set; }
    public string ClassName { get; set; }
    public string OrganizationCode { get; set; }
    public string OrganizationName { get; set; }

    public DateTime? AbsenceDate { get; set; }
    public DateTime? EventTime { get; set; }
    public int? AttendenceSection { get; set; }
}

