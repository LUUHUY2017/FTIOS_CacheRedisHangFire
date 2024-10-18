using Server.Core.Entities.GIO;

namespace Server.Core.Interfaces.ScheduleSendEmails.Responses;

public class VehicleInOutReportResponse : GIO_VehicleInOut
{
    public string? VehicleInOutStatusName { get; set; }
    public string? LaneInName { get; set; }
    public string? LaneOutName { get; set; }

    public int? No { get; set; }
    public string? Lpr { get; set; }

}

public class RangeHour
{
    public int Id { get; set; }
    public string? TimePeriod { get; set; }
}
