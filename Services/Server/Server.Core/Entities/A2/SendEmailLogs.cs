using Shared.Core.Entities;

namespace Server.Core.Entities.A2;

public class SendEmailLogs : EntityBase
{

    public string? SendEmailId { get; set; }
    public DateTime? TimeSent { get; set; }
    public string? MessageLog { get; set; }
    public bool? Sent { get; set; }
}
