namespace Shared.Core.Emails.V1.Commons;

public class MailRequest
{
    public string FromEmail { get; set; }
    /// <summary>
    /// Các email cách nhau bởi dấu ';'
    /// </summary>
    public string ToEmail { get; set; }
    public string DisplayName { get; set; }
    /// <summary>
    /// Các email cách nhau bởi dấu ';'
    /// </summary>
    public string ToBCC { get; set; }
    public string EmailSubject { get; set; }
    public string EmailBody { get; set; }
    public List<string> AttachFiles { get; set; }
}
