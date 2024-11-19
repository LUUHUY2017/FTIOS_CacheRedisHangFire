using Server.Core.Entities.A2;
using System.ComponentModel.DataAnnotations;

namespace Server.Application.MasterDatas.A2.DashBoards.V1.Models.Devices;

public class DashBoardDevice
{
    public string? Id { get; set; }
    public string? SerialNumber { get; set; }
    public string? DeviceName { get; set; }
    public string? IPAddress { get; set; }
    public string? DeviceDescription { get; set; }
    public int? HTTPPort { get; set; }
    public int? PortConnect { get; set; }

    public bool? ConnectionStatus { get; set; }
    public bool? Actived { get; set; }
    public string? DeviceModel { get; set; }
    public string? OrganizationId { get; set; }
    public string? OrganizationName { get; set; }
    public DashBoardDevice()
    {
        
    }
    public DashBoardDevice(Device d, Organization o)
    {
        Id = d?.Id;
        SerialNumber = d?.SerialNumber;
        DeviceName = d?.DeviceName;
        IPAddress = d?.IPAddress;
        DeviceDescription = d?.DeviceDescription;
        ConnectionStatus = d?.ConnectionStatus;
        DeviceModel = d?.DeviceModel;
        HTTPPort = d?.HTTPPort;
        PortConnect = d?.PortConnect;
        Actived = d?.Actived;

        OrganizationId = d?.OrganizationId;
        OrganizationName = o?.OrganizationName;
    }
}
