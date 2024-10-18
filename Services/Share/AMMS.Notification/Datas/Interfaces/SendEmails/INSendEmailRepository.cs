using AMMS.Notification.Datas.Entities;
using Shared.Core.Commons;

namespace AMMS.Notification.Datas.Interfaces.SendEmails;
public interface INSendEmailRepository
{
    Task<List<SendEmail>> GetAlls();
    Task<Result<SendEmail>> UpdateAsync(SendEmail data);
    Task<Result<int>> DeleteAsync(Guid id);
}



