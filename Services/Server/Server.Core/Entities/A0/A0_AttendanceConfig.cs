
using Shared.Core.Entities;

namespace Server.Core.Entities.A0;

public class A0_AttendanceConfig : EntityBase
{
    public string? EndpointIdentity { get; set; }
    public string? AccountName { get; set; }
    public string? Password { get; set; }
    public string? EndpointGateway { get; set; }

    //public TimeSpan? MorningStartTime { get; set; }
    //public TimeSpan? MorningEndTime { get; set; }
    //public TimeSpan? MorningLateTime { get; set; }
    //public TimeSpan? MorningBreakTime { get; set; }

    //public TimeSpan? AfternoonStartTime { get; set; }
    //public TimeSpan? AfternoonEndTime { get; set; }
    //public TimeSpan? AfternoonLateTime { get; set; }
    //public TimeSpan? AfternoonBreakTime { get; set; }

    public string? Note {  get; set; }
}
