
using Shared.Core.Commons;

namespace Server.Core.Interfaces.TimeAttendenceEvents.Requests;

public class AttendenceSyncReportFilterReq : BaseReportRequest
{
    public string? ClassId { get; set; }

}
