namespace Server.Application.MasterDatas.A2.DashBoards.V1.Models.Devices;

public class TotalDeviceModel
{
    public string? OrganizationId { get; set; }
    public string? OrganizationName { get; set; }
    public int? TotalSchool { get; set; } = 0;
    public int? TotalDevice { get; set; } = 0;
    public int? TotalOnline { get; set; } = 0;
    public int? TotalOffline { get; set; } = 0;
}
