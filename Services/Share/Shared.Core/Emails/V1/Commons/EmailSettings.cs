namespace Shared.Core.Emails.V1.Commons;

public class EmailSettings
{
    public string? EmailFrom { get; set; }
    public string? SmtpUser { get; set; }
    public string? SmtpPass { get; set; }
    public string? DisplayName { get; set; }
    public string? SmtpHost { get; set; }
    public int SmtpPort { get; set; }

}
