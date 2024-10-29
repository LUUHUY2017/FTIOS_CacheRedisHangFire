
using Shared.Core.Commons;

namespace Server.Core.Interfaces.TimeAttendenceEvents.Requests;

public class AttendenceReportFilterReq : BaseReportRequest
{
    public string? ClassId { get; set; }

}
