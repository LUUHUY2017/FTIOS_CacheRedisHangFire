﻿namespace Server.API.APIs.Data.ScheduleSendMails.V1.Requests;

public class ScheduleSendEmailLogFilter
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public string? Sent { get; set; } = "";
    public string? OrganizationId { get; set; } = "0";

}


