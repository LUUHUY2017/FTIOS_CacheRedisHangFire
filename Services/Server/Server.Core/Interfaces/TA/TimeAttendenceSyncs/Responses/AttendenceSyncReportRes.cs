using Server.Core.Entities.TA;

namespace Server.Core.Interfaces.TimeAttendenceSyncs.Responses;

public class AttendenceSyncReportRes : TimeAttendenceSync
{
    public string? StudentCode { get; set; }
    public string? Name { get; set; }
    public string? StudentName { get; set; }
    public string? ClassName { get; set; }
    public string? OrganizationCode { get; set; }
    public string? OrganizationName { get; set; }

    public DateTime? AbsenceDate { get; set; }
    public DateTime? EventTime { get; set; }
    public int? AttendenceSection { get; set; }
    /// <summary>
    /// Sự kiện
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

