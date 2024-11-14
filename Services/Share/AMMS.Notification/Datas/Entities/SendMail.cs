namespace AMMS.Notification.Datas.Entities;

public partial class SendEmail
{
    public Guid Id { get; set; }

    public string? OrganizationId { get; set; }

    public DateTime CreateTime { get; set; }

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

