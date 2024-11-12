namespace Server.Application.MasterDatas.A0.AttendanceTimeConfigs.V1.Models;

public class AttendanceTimeConfigRequest
{
    public string? Id { get; set; }
    public string? OrganizationId { get; set; } = "";
    //public string? OrganizationName{ get; set; } = "";
    public string? RollCallName { get; set; }

    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }

    public string? Note { get; set; }
}
