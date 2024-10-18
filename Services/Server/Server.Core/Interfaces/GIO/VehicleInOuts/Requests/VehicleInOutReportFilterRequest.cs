
using Shared.Core.Commons;

namespace Server.Core.Interfaces.ScheduleSendEmails.Requests;

public class VehicleInOutReportRequest : BaseReportRequest
{
    //public int VehicleInOutStatus { get; set; } = 0;
    public string? LaneId { get; set; }
    public string? PlateNumber { get; set; }

}
