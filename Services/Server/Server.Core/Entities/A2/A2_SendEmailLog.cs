using Shared.Core.Entities;

namespace Server.Core.Entities.A2;

public class A2_SendEmailLog : EntityBase
{

    public string? SendEmailId { get; set; }
    public DateTime? TimeSent { get; set; }
    public string? MessageLog { get; set; }
    public bool? Sent { get; set; }
}
