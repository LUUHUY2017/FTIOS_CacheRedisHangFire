using AMMS.Notification.Datas.Entities;
using AMMS.Notification.Datas.Interfaces.SendEmails;
using AMMS.Shared.Commons;
using Microsoft.EntityFrameworkCore;
using Shared.Core.Commons;

namespace AMMS.Notification.Datas.Repositories.SendEmails;

public class NSendEmailLogRepository : INSendEmailLogRepository
{
    private readonly NotificationDbContext _notiDbContext;
    public NSendEmailLogRepository(NotificationDbContext notiDbContext)
    {
        _notiDbContext = notiDbContext;
    }

    public Task<List<SendEmailLog>> GetAlls()
    {
        var _data = (from _do in _notiDbContext.SendEmailLogs
                     where true
                     select _do).ToListAsync();
        return _data;
    }

    public async Task<Result<SendEmailLog>> UpdateAsync(SendEmailLog data)
    {
        string message = "";
        try
        {
            var _order = _notiDbContext.SendEmailLogs.FirstOrDefault(o => o.Id == data.Id);
            if (_order != null)
            {
                CopyProperties.CopyPropertiesTo(data, _order);
                _notiDbContext.SendEmailLogs.Update(_order);
                message = "Cập nhật thành công";
            }
            else
            {
                _order = new SendEmailLog();
                CopyProperties.CopyPropertiesTo(data, _order);
                _notiDbContext.SendEmailLogs.Add(_order);
                message = "Thêm mới thành công";
            }
            var retVal = await _notiDbContext.SaveChangesAsync();
            return new Result<SendEmailLog>(data, message, true);
        }
        catch (Exception ex)
        {
            return new Result<SendEmailLog>(data, "Lỗi: " + ex.ToString(), false);
        }
    }

    public async Task<Result<int>> DeleteAsync(Guid id)
    {
        try
        {
            var result = _notiDbContext.SendEmailLogs.FirstOrDefault(o => o.Id == id);
            if (result == null)
                return new Result<int>("Không tìm thấy dữ liệu", false);

            _notiDbContext.SendEmailLogs.Remove(result);
            var retVal = _notiDbContext.SaveChanges();

            return new Result<int>(retVal, "Xóa thành công!", true);
        }
        catch (Exception ex)
        {
            return new Result<int>(0, "Lỗi: " + ex.ToString(), false);
        }
    }


}