using Shared.Core.Commons;

namespace Server.Core.Interfaces.A2.StudentClassRoomYears.Requests;

public class ClassRoomYearFilterRequest : BaseFilterRequest
{
    public string? SchoolYearId { get; set; } = "0";
    public string? OrganizationId { get; set; } = "0";
}