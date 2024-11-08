namespace AMMS.VIETTEL.SMAS.APIControllers.ScheduleSendMails.V1.Requests;

public class SendEmailRequest
{
    public Guid? Id { get; set; }

    public int? OrganizationId { get; set; }

    public DateTime? CreateTime { get; set; }

    public int? EmailSenderId { get; set; }

    public string? ToEmails { get; set; } = null!;

    public string? CcEmails { get; set; }

    public string? BccEmails { get; set; }

    public string? Subject { get; set; } = null!;

    public string? Body { get; set; } = null!;

    public bool? Sent { get; set; }

    public DateTime? TimeSent { get; set; }

    public int? NumberOfResend { get; set; }

    public string? AttachFile { get; set; }
}
