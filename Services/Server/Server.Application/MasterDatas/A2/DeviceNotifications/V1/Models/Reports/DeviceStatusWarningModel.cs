using Server.Core.Entities.A2;
using System.ComponentModel.DataAnnotations;

namespace Server.Application.MasterDatas.A2.DeviceNotifications.V1.Models.Reports;

public class DeviceStatusWarningModel
{
    public string? SerialNumber { get; set; }

    public string? DeviceName { get; set; }

    public string? IPAddress { get; set; }


    public DateTime? CheckConnectTime { get; set; }
    public DateTime? ConnectUpdateTime { get; set; }
    public DateTime? DisConnectUpdateTime { get; set; }

    public bool? ConnectionStatus { get; set; }

    /// <summary>
    /// Model thiết bị  
    /// </summary>
    public string? DeviceModel { get; set; }
    public string? OrganizationId { get; set; }
    public string? OrganizationName { get;set; }

    public DeviceStatusWarningModel()
    {
        
    }

    public DeviceStatusWarningModel(Device d, Organization o)
    {
        OrganizationId = d.OrganizationId;
        SerialNumber = d.SerialNumber;
        DeviceName = d.DeviceName;
        IPAddress = d.IPAddress;
        CheckConnectTime = d.CheckConnectTime;
        ConnectUpdateTime = d.ConnectUpdateTime;
        DisConnectUpdateTime = d.DisConnectUpdateTime;
        ConnectionStatus = d.ConnectionStatus;
        DeviceModel = d.DeviceModel;
        OrganizationName = o.OrganizationName;
    }
}
