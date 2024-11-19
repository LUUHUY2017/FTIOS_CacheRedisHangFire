namespace Server.Application.MasterDatas.A2.ClassRooms.V1.Models;

public class ClassRoomRequest
{
    public string? Id { get; set; }

    public string? OrganizationId { get; set; }

    /// <summary>
    /// Trạng thái
    /// </summary>
    public bool? Actived { get; set; }
    public string? Name { get; set; }
}
