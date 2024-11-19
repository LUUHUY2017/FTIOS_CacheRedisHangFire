using Server.Core.Entities.A2;

namespace Server.Core.Interfaces.A2.StudentClassRoomYears.Reponses;
public class ClassRoomYearReportResponse : StudentClassRoomYear
{
    public string? OrganizationName { get; set; }

    public string? SchoolYearName { get; set; }
    public string? ClassRoomName { get; set; }

}
