using Shared.Core.Commons;
using Shared.Core.Repositories;

namespace AMMS.Notification.Datas;

public interface INotificationRepository : IAsyncRepository<Notification>
{
    Task<IEnumerable<Notification>> GetNotificationByUserId(string userId);
    Task<IEnumerable<Notification>> GetNotificationByUserName(string userName);
    Task<Result<Notification>> Readed(string id);

    Task<Result<List<Notification>>> ReadAll(string userId);
}
