namespace Server.Core.Interfaces.A2.ScheduleSendEmails.Requests;

public class ScheduleSendEmailModel
{
    public string? OrganizationId { get; set; } = "0";
    public string? ColumnTable { get; set; }
    public string? Actived { get; set; }
    public string? Key { get; set; }
    public string? Export { get; set; } = "0";
    /// <summary>
    /// Loại lập lịch
    /// </summary>
    public string? ScheduleNote { get; set; }

}
public class ScheduleSendEmailLogModel
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Sent { get; set; } = "";
    public string? OrganizationId { get; set; } = "0";

}
