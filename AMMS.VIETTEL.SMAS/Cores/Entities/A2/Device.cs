using Shared.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace AMMS.VIETTEL.SMAS.Cores.Entities.A2;

public class Device : EntityBase
{
    [MaxLength(250)]
    public string? DeviceCode { get; set; }
    public string? DeviceName { get; set; }
    public string? DeviceParam { get; set; }
    public string? DeviceInfo { get; set; }

    public string? IPAddress { get; set; }
    public string? KeyLicense { get; set; }
    public string? KeyConnect { get; set; }
    public string? DeviceType { get; set; }

    [MaxLength(500)]
    public string? DeviceDescription { get; set; }
    public string? Param { get; set; }
    [MaxLength(50)]
    public string? SerialNumber { get; set; }
    public string? MacAddress { get; set; }

    public int? HTTPPort { get; set; }
    public int? PortConnect { get; set; }
    public string? LaneId { get; set; }
    public string? GateId { get; set; }
    public DateTime? CheckConnectTime { get; set; }
    public DateTime? ConnectUpdateTime { get; set; }
    public DateTime? DisConnectUpdateTime { get; set; }

    [MaxLength(50)]
    public string? ActiveKey { get; set; }
    public string? DeviceID { get; set; }
    public string? Password { get; set; }
    public string? BrandName { get; set; }

    public bool? ConnectionStatus { get; set; }
    /// <summary>
    /// Tín hiệu vào true - false
    /// </summary>
    public bool? DeviceIn { get; set; }
    /// <summary>
    /// Tín hiệu ra true - false
    /// </summary>
    public bool? DeviceOut { get; set; }
    /// <summary>
    /// Tín hiệu ra dạng chuỗi
    /// </summary>
    public bool? DeviceReader { get; set; }
    /// <summary>
    /// Loại thiết bị vào true
    /// </summary>
    public bool? DeviceInput { get; set; }

    /// <summary>
    /// Model thiết bị  
    /// </summary>
    public string? DeviceModel { get; set; }

}


