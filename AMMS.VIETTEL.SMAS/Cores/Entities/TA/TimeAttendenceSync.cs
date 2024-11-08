using Shared.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMMS.VIETTEL.SMAS.Cores.Entities.TA;

[Table("TimeAttendenceSync")]

public class TimeAttendenceSync : EntityBase
{
    public string? TimeAttendenceEventId { get; set; }
    public string? ParamRequests { get; set; }
    public string? ParamResponses { get; set; }
    public bool? SyncStatus { get; set; }
    public string? Message { get; set; }
}
