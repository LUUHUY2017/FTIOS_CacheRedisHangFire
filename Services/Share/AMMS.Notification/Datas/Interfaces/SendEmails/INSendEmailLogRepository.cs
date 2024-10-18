using AMMS.Notification.Datas.Entities;
using Shared.Core.Commons;

namespace AMMS.Notification.Datas.Interfaces.SendEmails;
public interface INSendEmailLogRepository
{
    Task<List<SendEmailLog>> GetAlls();
    Task<Result<SendEmailLog>> UpdateAsync(SendEmailLog data);
    Task<Result<int>> DeleteAsync(Guid id);
}



