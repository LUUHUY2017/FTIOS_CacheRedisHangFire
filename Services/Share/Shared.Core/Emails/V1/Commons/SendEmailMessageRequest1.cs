namespace Shared.Core.Emails.V1.Commons;

public class SendEmailMessageRequest1
{
    public string Id { get; set; }
    /// <summary>
    /// Chanel trả lại kết quả khi gửi email
    /// </summary>
    public string EventChanelReturnSendEmailResponse { get; set; }

    /// <summary>
    /// Phương thức gửi email 
    /// </summary>
    public EmailSendingMethods EmailSendingMethod { get; set; }

    /// <summary>
    /// Thông tin email gửi
    /// </summary>
    public EmailSettings EmailSettings { get; set; }
    public MailRequest Message { get; set; }
    public string DisplayName { get; set; }
}
