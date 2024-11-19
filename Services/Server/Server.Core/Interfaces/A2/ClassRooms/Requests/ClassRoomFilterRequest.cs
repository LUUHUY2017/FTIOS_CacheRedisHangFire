using Shared.Core.Commons;

namespace Server.Core.Interfaces.A2.ClassRooms.Requests;

public class ClassRoomFilterRequest : BaseFilterRequest
{
    public string? OrganizationId { get; set; } = "0";
}