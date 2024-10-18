using Microsoft.EntityFrameworkCore;

namespace AMMS.Notification.Datas;

public interface INotificationDbContext
{
    public DbSet<Notification> Notification { get; set; }
}
