using Shared.Core.Entities;

namespace Server.Core.Entities.A2;

public partial class A2_SendEmail : EntityBase
{

    public string? EmailSenderId { get; set; }

    public string ToEmails { get; set; } = null!;

    public string? CcEmails { get; set; }

    public string? BccEmails { get; set; }

    public string Subject { get; set; } = null!;

    public string Body { get; set; } = null!;

    public bool? Sent { get; set; }

    public DateTime? TimeSent { get; set; }

    public int? NumberOfResend { get; set; }

    public string? AttachFile { get; set; }
}
