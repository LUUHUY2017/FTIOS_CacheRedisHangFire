using Newtonsoft.Json;
using Shared.Core.Commons;
using Shared.Core.Emails.V1.Commons;
using Shared.Core.Loggers;
using System.Net;
using System.Net.Mail;

namespace Shared.Core.Emails.V1.Adapters;


public class SMTP_Email_Adpater
{
    public EmailSettings _mailSettings { get; }

    public SMTP_Email_Adpater() { }


    public async Task<Result<string>> SendEmailAsync(SendEmailMessageRequest1 mailRequest)
    {
        try
        {
            MailMessage message = new MailMessage();
            using (SmtpClient smtp = new SmtpClient())
            {
                if (string.IsNullOrEmpty(mailRequest.Message.FromEmail))
                    mailRequest.Message.FromEmail = "noreply@acs.vn";
                if (string.IsNullOrEmpty(mailRequest.Message.DisplayName))
                    mailRequest.Message.DisplayName = "ACS Analytics Platform";
                message.From = new MailAddress(mailRequest.Message.FromEmail, mailRequest.Message.DisplayName, System.Text.Encoding.UTF8);


                if (!string.IsNullOrEmpty(mailRequest.Message.ToEmail))
                {
                    var emails = mailRequest.Message.ToEmail.Split(new char[] { ';' });
                    if (emails.Length > 0)
                    {
                        foreach (var email in emails)
                        {
                            if (!string.IsNullOrEmpty(email) && email.Contains('@'))
                            {
                                message.Bcc.Add(new MailAddress(email));

                                //if (string.IsNullOrEmpty(mailRequest.Message.DisplayName))
                                message.To.Add(new MailAddress(email));
                                //else
                                //    message.To.Add(new MailAddress(email, mailRequest.Message.DisplayName));
                            }
                        }
                    }
                }


                if (!string.IsNullOrEmpty(mailRequest.Message.ToBCC))
                {
                    var emails = mailRequest.Message.ToBCC.Split(new char[] { ';' });
                    if (emails.Length > 0)
                    {
                        foreach (var email in emails)
                        {
                            if (!string.IsNullOrEmpty(email) && email.Contains('@'))
                            {
                                if (string.IsNullOrEmpty(mailRequest.Message.DisplayName))
                                    message.To.Add(new MailAddress(email));
                                else
                                    message.To.Add(new MailAddress(email, mailRequest.Message.DisplayName));
                            }

                        }
                    }
                }

                //message.Subject = "Email tự động thông báo thiết bị mất kết nối";
                message.Subject = mailRequest.Message.EmailSubject;

                message.IsBodyHtml = true; //to make message body as html  
                message.Body = mailRequest.Message.EmailBody;
                List<string> _attachFile = null;

                if (mailRequest.Message.AttachFiles != null && mailRequest.Message.AttachFiles.Count > 0)
                {
                    _attachFile = mailRequest.Message.AttachFiles;
                    if (_attachFile != null)
                    {
                        bool fileSuccess = true;
                        foreach (string files in _attachFile)
                        {
                            if (File.Exists(files))
                            {
                                System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(files);
                                message.Attachments.Add(attachment);
                            }
                            else
                            {
                                fileSuccess = false;
                            }
                        }
                        // Nếu có bất kì 1 file nào đó k tồn tại sẽ k gửi mail
                        if (!fileSuccess)
                        {
                            string listFile = JsonConvert.SerializeObject(_attachFile);
                            throw new Exception("File not found: " + listFile);
                        }
                    }
                }

                smtp.Port = mailRequest.EmailSettings.SmtpPort;

                smtp.Host = mailRequest.EmailSettings.SmtpHost;// "smtp.gmail.com";
                //smtp.Host = host;

                smtp.EnableSsl = true;
                //smtp.EnableSsl = enableSsl;

                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(mailRequest.EmailSettings.SmtpUser, mailRequest.EmailSettings.SmtpPass);

                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;


                smtp.Send(message);
                smtp.Dispose();
                // nếu có attach file
                //if (_attachFile != null)
                //{
                //    foreach (string files in _attachFile)
                //    {
                //        File.Delete(files);
                //    }
                //}
            }
            return new Result<string>("Đã gửi email thành công", true);
        }
        catch (Exception ex)
        {
            Logger.Error(ex);
            return new Result<string>("Lỗi khi gửi email", $"{ex.Message}", false);
        }
    }
}