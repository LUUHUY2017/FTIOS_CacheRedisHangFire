using Server.Core.Entities.TA;

namespace Server.Core.Interfaces.TimeAttendenceEvents.Responses;

public class AttendenceEventReportRes : TimeAttendenceEvent
{
    public string? StudentName { get; set; }
    public string? ClassName { get; set; }
    public string? DateOfBirth { get; set; }
    public string? DeviceName { get; set; }

    public string? OrganizationCode { get; set; }
    public string? OrganizationName { get; set; }
}

