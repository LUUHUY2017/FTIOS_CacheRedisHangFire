using AMMS.Notification.Datas.Entities;
using AMMS.Notification.Datas.Interfaces.SendEmails;
using AMMS.Shared.Commons;
using Microsoft.EntityFrameworkCore;
using Shared.Core.Commons;

namespace AMMS.Notification.Datas.Repositories.SendEmails;

public class NSendEmailRepository : INSendEmailRepository
{
    private readonly NotificationDbContext _notiDbContext;
    public NSendEmailRepository(NotificationDbContext notiDbContext)
    {
        _notiDbContext = notiDbContext;
    }

    public Task<List<SendEmail>> GetAlls()
    {
        var _data = (from _do in _notiDbContext.SendEmails
                     where true
                     select _do).ToListAsync();
        return _data;
    }

    public async Task<Result<SendEmail>> UpdateAsync(SendEmail data)
    {
        string message = "";
        try
        {
            var _order = _notiDbContext.SendEmails.FirstOrDefault(o => o.Id == data.Id);
            if (_order != null)
            {
                CopyProperties.CopyPropertiesTo(data, _order);
                _notiDbContext.SendEmails.Update(_order);
                message = "Cập nhật thành công";
            }
            else
            {
                _order = new SendEmail();
                CopyProperties.CopyPropertiesTo(data, _order);
                _notiDbContext.SendEmails.Add(_order);
                message = "Thêm mới thành công";
            }
            var retVal = await _notiDbContext.SaveChangesAsync();
            return new Result<SendEmail>(data, message, true);
        }
        catch (Exception ex)
        {
            return new Result<SendEmail>(data, "Lỗi: " + ex.ToString(), false);
        }
    }

    public async Task<Result<int>> DeleteAsync(Guid id)
    {
        try
        {
            var result = _notiDbContext.SendEmails.FirstOrDefault(o => o.Id == id);
            if (result == null)
                return new Result<int>("Không tìm thấy dữ liệu", false);

            _notiDbContext.SendEmails.Remove(result);
            var retVal = _notiDbContext.SaveChanges();

            return new Result<int>(retVal, "Xóa thành công!", true);
        }
        catch (Exception ex)
        {
            return new Result<int>(0, "Lỗi: " + ex.ToString(), false);
        }
    }


}