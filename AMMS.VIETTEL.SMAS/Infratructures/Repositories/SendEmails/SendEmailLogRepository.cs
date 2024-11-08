using AMMS.Shared.Commons;
using AMMS.VIETTEL.SMAS.Cores.Entities.A2;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.ScheduleSendEmails;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.SendEmails;
using AMMS.VIETTEL.SMAS.Infratructures.Databases;
using Microsoft.EntityFrameworkCore;
using Shared.Core.Commons;

namespace AMMS.VIETTEL.SMAS.Infratructures.Repositories.SendEmails;


public class SendEmailLogRepository : ISendEmailLogRepository
{
    private readonly IViettelDbContext _db;
    public SendEmailLogRepository(IViettelDbContext biDbContext)
    {
        _db = biDbContext;
    }
    public async Task<Result<List<SendEmailLogs>>> Get(string sendEmailId)
    {
        try
        {
            var _data = await (from _do in _db.SendEmailLogs
                               where _do.SendEmailId == sendEmailId
                               select _do).ToListAsync();

            return new Result<List<SendEmailLogs>>(_data, "Thành công!", true);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    public async Task<Result<List<SendEmailLogs>>> GetAlls(ScheduleSendEmailLogModel request)
    {
        try
        {
            var _data = await (from _do in _db.SendEmailLogs
                               where
                                (request.StartDate != null ? _do.TimeSent.Value.Date >= request.StartDate.Value.Date : true)
                                && (request.EndDate != null ? _do.TimeSent.Value.Date <= request.EndDate.Value.Date : true)
                               orderby _do.TimeSent descending
                               select _do).ToListAsync();

            return new Result<List<SendEmailLogs>>(_data, "Xóa thành công!", true);
        }
        catch (Exception ex)
        {
            return new Result<List<SendEmailLogs>>("Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<Result<SendEmailLogs>> UpdateAsync(SendEmailLogs data)
    {
        string message = "";
        try
        {
            var _order = _db.SendEmailLogs.FirstOrDefault(o => o.Id == data.Id);
            if (_order != null)
            {
                data.CopyPropertiesTo(_order);
                _db.SendEmailLogs.Update(_order);
                message = "Cập nhật thành công";
            }
            else
            {
                _order = new SendEmailLogs();
                data.CopyPropertiesTo(_order);
                _db.SendEmailLogs.Add(_order);
                message = "Thêm mới thành công";
            }
            var retVal = await _db.SaveChangesAsync();
            return new Result<SendEmailLogs>(data, message, true);
        }
        catch (Exception ex)
        {
            return new Result<SendEmailLogs>(data, "Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<Result<int>> DeleteAsync(string id)
    {
        try
        {
            var result = _db.SendEmailLogs.FirstOrDefault(o => o.Id == id);
            if (result == null)
                return new Result<int>("Không tìm thấy dữ liệu", false);

            _db.SendEmailLogs.Remove(result);
            var retVal = await _db.SaveChangesAsync();

            return new Result<int>(retVal, "Xóa thành công!", true);
        }
        catch (Exception ex)
        {
            return new Result<int>(0, "Lỗi: " + ex.ToString(), false);
        }
    }

}
