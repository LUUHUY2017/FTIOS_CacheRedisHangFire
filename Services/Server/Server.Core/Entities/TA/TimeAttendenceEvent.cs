using Shared.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Core.Entities.TA;

[Table("TimeAttendenceEvent")]

namespace Server.Core.Entities.TA
{
    public class TimeAttendenceEvent : EntityBase
    {
        public string? DeviceId { get; set; }
        public string? DeviceIP { get; set; }

    public string? PersonId { get; set; }
    public string? EnrollNumber { get; set; }
    public DateTime? EventTime { get; set; }

    public string? InOutMode { get; set; }
    public string? ShiftCode { get; set; }
    public byte? GetMode { get; set; }

    public string? Description { get; set; }
    public bool? EventType { get; set; }
    public string? TAMessage { get; set; }

    public string? SchoolCode { get; set; }
    public string? SchoolYearCode { get; set; }
    public string? ClassCode { get; set; }

    public DateTime? AbsenceDate { get; set; }
    /// <summary>
    /// Buổi điểm danh: 0- sáng, 1- chiều, 2- tối
    /// </summary>
    public int? AttendenceSection { get; set; }
    /// <summary>
    /// Kiểu gửi tin nhắn:1: Gửi tin nhắn qua SMS và EduOne 2: Gửi thông báo qua EduOne 3: Gửi tin nhắn qua SMS
    /// </summary>
    public int? FormSendSMS { get; set; }
    public string? StudentCode { get; set; }

    /// <summary>
    /// Giá trị điểm danh, chỉ chấp nhập 4 giá trị:    
    /// C: Có mặtK: Nghỉ không phép
    /// K: Nghỉ không phép
    /// P: Nghỉ có phép
    /// X: Trường hợp đặc biệt(Đi muộn, bỏ tiết, về sớm)
    /// </summary>
    public string? ValueAbSent { get; set; }
}
