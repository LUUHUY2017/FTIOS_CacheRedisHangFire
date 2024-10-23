using Shared.Core.Entities;

namespace Server.Core.Entities.TA
{
    public class TA_TimeAttendenceEvent : EntityBase
    {
        public string? DeviceId { get; set; }
        public string? DeviceIP { get; set; }

        public string? PersonId { get; set; }
        public string? EnrollNumber { get; set; }
        public DateTime? EventTime { get; set; }

        public string? InOutMode { get; set; }
        public string? ShiftCode { get; set; }
        public byte? GetMode { get; set; }


        public string? Description { get; set; }
        public bool? EventType { get; set; }
        public string? TAMessage { get; set; }
    }
}
