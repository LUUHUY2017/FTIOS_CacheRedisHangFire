using Shared.Core.Entities;

namespace Server.Core.Entities.TA
{
    public class TA_TimeAttendenceDetail : EntityBase
    {
        public string? TA_TimeAttendenceEventId { get; set; }
        public bool? IsLate { get; set; }
        public bool? IsOffSoon { get; set; }
        public bool? IsOffPeriod { get; set; }
        public DateTime? LateTime { get; set; }
        public DateTime? OffSoonTime { get; set; }
        public bool? PeriodI { get; set; }
        public bool? PeriodII { get; set; }
        public bool? PeriodIII { get; set; }
        public bool? PeriodIV { get; set; }
        public bool? PeriodV { get; set; }
        public bool? PeriodVI { get; set; }
        public DateTime? AbsenceTime { get; set; }
    }

}
