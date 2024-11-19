namespace Server.Application.MasterDatas.A2.DashBoards.V1.Models.Devices;

public class TotalDeviceOrgModel
{
    public string? OrganizationId { get; set; }
    public string? OrganizationName { get; set; }
    public int? TotalDevice { get; set; } = 0;
    public int? TotalZK { get; set; } = 0;
    public int? TotalHN { get; set; } = 0;
}
