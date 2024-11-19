namespace Server.Application.MasterDatas.A2.ClassRoomYears.V1.Models;

public class ClassRoomYearRequest
{
    public string? Id { get; set; }
    /// <summary>
    /// Trạng thái
    /// </summary>
    public bool? Actived { get; set; }
    public string? Name { get; set; }
    public string? OrganizationId { get; set; }
    public string? SchoolId { get; set; }
    public string? ClassRoomId { get; set; }
    public string? SchoolYearId { get; set; }


}
