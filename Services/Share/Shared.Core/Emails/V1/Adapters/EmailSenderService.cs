using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using Shared.Core.Emails.V1.Commons;
using Shared.Core.Exceptions;
using Shared.Core.Loggers;

namespace Shared.Core.Emails.V1.Adapters;

public class EmailSenderService : IEmailSender
{
    public EmailSettings _mailSettings { get; }

    public EmailSenderService(IOptions<EmailSettings> mailSettings)
    {
        _mailSettings = mailSettings.Value;
    }
    public async Task SendEmailAsync(string toEmail, string subject, string htmlMessage)
    {
        //var a = Console.ReadLine();

        try
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.EmailFrom);
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;
            var builder = new BodyBuilder();
            builder.HtmlBody = htmlMessage;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.SmtpHost, _mailSettings.SmtpPort, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.SmtpUser, _mailSettings.SmtpPass);
            //smtp.Authenticate("nguyencongquyet@gmail.com", "tiax zdkq envs rsjv");
            //smtp.Authenticate("info@amms.acs.vn", "fyih rvcy qglw mjto");
            
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
            //throw new ApiException(ex.Message);
            throw new ApiException($"{_mailSettings.SmtpUser}:{_mailSettings.SmtpPass}");
        }
    }
}
