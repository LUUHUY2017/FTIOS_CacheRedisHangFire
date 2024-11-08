using Shared.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Core.Entities.GIO
{
    public class VehicleInOut : EntityBase
    {
        public string? VehicleId { get; set; }
        [MaxLength(20)]
        public string? VehicleCode { get; set; }
        [MaxLength(20)]
        public string? VehicleName { get; set; }
        public string? PlateNumber { get; set; }
        // Đã nhận dạng biển số
        public string? PlateNumberIdentification { get; set; }
        public string? PlateNumberIdentificationOut { get; set; }


    public DateTime? DateTimeIn { get; set; }
    public DateTime? DateTimeOut { get; set; }

    public string? LaneInId { get; set; }
    public string? LaneOutId { get; set; }

    public string? GateInId { get; set; }
    public string? GateOutId { get; set; }


    public string? UserInId { get; set; }
    public string? UserOutId { get; set; }

    /// <summary>
    /// 1: In, 2:Out
    /// </summary>
    public int? VehicleInOutStatus { get; set; }

    // Số thứ tự xe vào trong ngày
    public int? VehicleNoInDay { get; set; }
    [MaxLength(500)]
    public string? Note { get; set; }


    public string? PlateColor { get; set; }
    public string? VehicleType { get; set; }
    public string? VehicleColor { get; set; }
    public string? Speed { get; set; }
    public string? Direction { get; set; }
    public string? Location { get; set; }
    public string? Region { get; set; }
}
