using Server.Core.Entities.TA;

namespace Server.Core.Interfaces.TimeAttendenceSyncs.Responses;

public class AttendenceSyncReportRes : TA_TimeAttendenceSync
{
    public string StudentCode { get; set; }
    public string StudentName { get; set; }
    public string ClassName { get; set; }
    public string SchoolName { get; set; }

    public DateTime? AbsenceDate { get; set; }
    public int? AttendenceSection { get; set; }
}

