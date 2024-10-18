namespace Server.Application.MasterDatas.A2.AutoReportInOut.V1.Models;

public class ScheduleReportInOutFilter
{
    public int? OrganizationId { get; set; } = 0;
    public string? ColumnTable { get; set; }
    public string? Actived { get; set; }
    public string? Key { get; set; }
    public string? Export { get; set; } = "0";
    /// <summary>
    /// Loại lập lịch
    /// </summary>
    public string? ScheduleNote { get; set; }
}
