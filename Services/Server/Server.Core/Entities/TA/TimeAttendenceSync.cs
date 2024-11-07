using Shared.Core.Entities;

namespace Server.Core.Entities.TA
{
    public class TimeAttendenceSync : EntityBase
    {
        public string? TimeAttendenceEventId { get; set; }
        public string? ParamRequests { get; set; }
        public string? ParamResponses { get; set; }
        public bool? SyncStatus { get; set; }
        public string? Message { get; set; }
    }
}
