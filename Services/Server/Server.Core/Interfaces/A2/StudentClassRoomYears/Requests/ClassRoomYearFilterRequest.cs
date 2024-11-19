using Shared.Core.Commons;

namespace Server.Core.Interfaces.A2.StudentClassRoomYears.Requests;

public class ClassRoomYearFilterRequest : BaseFilterRequest
{
    public string? OrganizationId { get; set; } = "0";
}