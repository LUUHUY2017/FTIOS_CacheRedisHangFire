using Microsoft.EntityFrameworkCore;
using Shared.Core.Commons;

namespace AMMS.Notification.Datas;

public class NotificationRepository : RepositoryBaseNotificationData<Notification>, INotificationRepository
{
    public NotificationRepository(NotificationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<Notification>> GetNotificationByUserId(string userId)
    {
        var orderList = await _dbContext.Notification
           .Where(o => o.UserId == userId)
           .ToListAsync();
        return orderList;
    }

    public async Task<IEnumerable<Notification>> GetNotificationByUserName(string userName)
    {
        var orderList = await _dbContext.Notification
           .Where(o => o.UserId == userName)
           .ToListAsync();
        return orderList;
    }

    public async Task<Result<List<Notification>>> ReadAll(string userId)
    {
        var entities = await _dbContext.Notification.Where(o => o.UserId == userId && o.Readed != true).ToListAsync();
        if (entities != null)
            foreach (var entity in entities)
                entity.Readed = true;
        var val = await _dbContext.SaveChangesAsync();
        if (val > 0)
        {
            return new Result<List<Notification>>(entities, "Thành công", true);
        }
        return new Result<List<Notification>>(entities, "Thành công! Không có sự thay đổi CSDL", true);
    }

    public async Task<Result<Notification>> Readed(string id)
    {
        var entity = await _dbContext.Notification.FirstOrDefaultAsync(o => o.Id == id);
        if (entity != null)
            entity.Readed = true;
        var val = await _dbContext.SaveChangesAsync();
        if (val > 0)
            return new Result<Notification>(entity, "Thành công", true);
        return new Result<Notification>(entity, "Có lỗi khi cập nhậ trạng thái", false);
    }
}
