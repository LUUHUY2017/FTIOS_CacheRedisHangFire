using Server.Core.Entities.TA;

namespace Server.Core.Interfaces.TimeAttendenceEvents.Responses;

public class AttendenceEventReportRes : TA_TimeAttendenceEvent
{
    public string StudentName { get; set; }
    public string ClassName { get; set; }
}

