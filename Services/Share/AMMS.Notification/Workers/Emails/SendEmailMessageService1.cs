using AMMS.Notification.Datas;
using AMMS.Notification.Datas.Entities;
using AMMS.Notification.Datas.Interfaces.SendEmails;
using Shared.Core.Commons;
using Shared.Core.Emails.V1;
using Shared.Core.Emails.V1.Commons;
using Shared.Core.Loggers;

namespace AMMS.Notification.Workers.Emails;

public class SendEmailMessageService1
{
    private readonly NotificationDbContext _notiDbContext;
    private readonly EmailSenderServiceV1 _emailSenderServiceV1;
    private readonly INSendEmailRepository _sendEmailRepository;
    private readonly INSendEmailLogRepository _sendEmailLogRepository;
    public SendEmailMessageService1(EmailSenderServiceV1 emailSenderServiceV1,
        NotificationDbContext notiDbContext,
        INSendEmailRepository sendEmailRepository,
        INSendEmailLogRepository sendEmailLogRepository)
    {
        _notiDbContext = notiDbContext;
        _emailSenderServiceV1 = emailSenderServiceV1;
        _sendEmailRepository = sendEmailRepository;
        _sendEmailLogRepository = sendEmailLogRepository;
    }
    /// <summary>
    /// Gửi email qua RabbitMQ
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<Result<string>> SendByEventBusAsync(SendEmailMessageRequest1 request)
    {

        //Gửi email
        var retVal = await _emailSenderServiceV1.SendByEventBusAsync(request);

        //Lưu dữ liệu vào CSDL
        //await UpdateSendStatus(new SendEmailMessageResponse1() { Id = request.Id, Message = retVal.Message, SendStatus = retVal.Succeeded });

        return retVal;
    }
    /// <summary>
    /// Gửi email trực tiếp
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<Result<string>> SendAsync(SendEmailMessageRequest1 request)
    {

        //Gửi email
        var retVal = await _emailSenderServiceV1.SendAsync(request);
        //Lưu dữ liệu vào CSDL
        await UpdateSendStatus(new SendEmailMessageResponse1() { Id = request.Id, Message = retVal.Message, SendStatus = retVal.Succeeded });
        return retVal;
    }


    public async Task UpdateSendStatus(SendEmailMessageResponse1 response)
    {
        try
        {
            Console.WriteLine("Cap nhat ket qua gui email qua event bus");

            // Nếu kết quả gửi thành công -> Cập nhật vào bảng Lịch sử và kèm ghi Logs
            //Guid id = Guid.Parse(response.Id);
            var sends = _notiDbContext.SendEmails.FirstOrDefault(o => o.Id == response.Id);
            if (sends != null)
            {
                sends.Sent = response.SendStatus;
                sends.NumberOfResend = 1;
                sends.TimeSent = DateTime.Now;
                await _sendEmailRepository.UpdateAsync(sends);

                var sendLog = new SendEmailLog()
                {
                    //Id = Guid.NewGuid(),
                    Sent = response.SendStatus,
                    SendEmailId = sends.Id,
                    TimeSent = DateTime.Now,
                    MessageLog = response.Message,
                };
                await _sendEmailLogRepository.UpdateAsync(sendLog);
            }
            // Nếu kết quả không thành công lưu vào Redis với cơ chế Pub/Sub để đẩy vào Queue
            if (!response.SendStatus)
            {


            }




        }
        catch (Exception ex)
        {
            Logger.Error("Lỗi cập nhật kết quả gửi email");
            Logger.Error(ex);
        }
    }
}
