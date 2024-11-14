using Server.Core.Entities.A2;
using Shared.Core.Commons;

namespace Server.Core.Interfaces.A2.DeviceNotifications
{
    public interface IDeviceNotificationRepository
    {
        Task<Result<ScheduleSendMail>> GetById(string id);
        Task<List<ScheduleSendMail>> GetAlls(DeviceNotificationModel request);
        Task<Result<ScheduleSendMail>> UpdateAsync(ScheduleSendMail data);
        Task<Result<ScheduleSendMail>> ActiveAsync(ActiveRequest data);
        Task<Result<ScheduleSendMail>> InActiveAsync(InactiveRequest data);
    }
}
