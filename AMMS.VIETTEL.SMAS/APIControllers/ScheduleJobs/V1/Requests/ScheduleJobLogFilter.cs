namespace Server.API.APIs.Data.ScheduleJobs.V1.Requests;

public class ScheduleJobLogFilter
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public string? Sent { get; set; } = "";
    public int? OrganizationId { get; set; } = 0;

}


