namespace Server.Application.MasterDatas.A2.DashBoards.V1.Models.Devices;

public class DBDeviceFilter
{
    public string? ColumnTable { get; set; } = null;
    public string? Key { get; set; } = null;

    public string? Actived { get; set; }
    public string? Status { get; set; }
    public string? OrganizationId { get; set; }
    public string? DeviceModel { get; set; }
}
