using Shared.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace AMMS.VIETTEL.SMAS.Cores.Entities.TA;

public class TimeAttendenceEvent : EntityBase
{
    public string? DeviceId { get; set; }
    public string? DeviceIP { get; set; }

    public string? PersonId { get; set; }
    [MaxLength(50)]
    public string? EnrollNumber { get; set; }
    public DateTime? EventTime { get; set; }



    public string? InOutMode { get; set; }
    public string? ShiftCode { get; set; }
    public byte? GetMode { get; set; }

    public string? Description { get; set; }
    public bool? EventType { get; set; }
    public string? TAMessage { get; set; }

    [MaxLength(50)]
    public string? StudentCode { get; set; }
    [MaxLength(50)]
    public string? SchoolCode { get; set; }
    public string? SchoolYearCode { get; set; }
    [MaxLength(50)]
    public string? ClassCode { get; set; }

    /// <summary>
    /// Ngày điểm danh
    /// </summary>
    public DateTime? AbsenceDate { get; set; }
    /// <summary>
    /// Buổi điểm danh: 0- sáng, 1- chiều, 2- tối
    /// </summary>
    public int? AttendenceSection { get; set; }
    /// <summary>
    /// Kiểu gửi tin nhắn:1: Gửi tin nhắn qua SMS và EduOne 2: Gửi thông báo qua EduOne 3: Gửi tin nhắn qua SMS
    /// </summary>
    public int? FormSendSMS { get; set; }
    /// <summary>
    /// Giá trị điểm danh, chỉ chấp nhập 4 giá trị:    
    /// C: Có mặtK: Nghỉ không phép
    /// K: Nghỉ không phép
    /// P: Nghỉ có phép
    /// X: Trường hợp đặc biệt(Đi muộn, bỏ tiết, về sớm)
    /// </summary>
    public string? ValueAbSent { get; set; }
    /// <summary>
    /// Đi muộn
    /// </summary>
    public bool? IsLate { get; set; }
    /// <summary>
    /// Về sớm
    /// </summary>
    public bool? IsOffSoon { get; set; }
    /// <summary>
    /// Thời gian đi muộn
    /// </summary>
    public DateTime? LateTime { get; set; }
    /// <summary>
    /// Thời gian về sớm
    /// </summary>
    public DateTime? OffSoonTime { get; set; }
}