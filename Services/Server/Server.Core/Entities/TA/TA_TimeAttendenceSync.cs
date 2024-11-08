using Shared.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Core.Entities.TA;

[Table("TimeAttendenceSync")]

public class TA_TimeAttendenceSync : EntityBase
{
    public string? TimeAttendenceEventId { get; set; }
    public string? ParamRequests { get; set; }
    public string? ParamResponses { get; set; }
    public bool? SyncStatus { get; set; }
    public string? Message { get; set; }
}
