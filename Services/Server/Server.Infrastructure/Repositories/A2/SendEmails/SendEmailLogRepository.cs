using AMMS.Shared.Commons;
using Microsoft.EntityFrameworkCore;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.ScheduleSendEmails.Requests;
using Server.Core.Interfaces.A2.SendEmails;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.Commons;

namespace Server.Infrastructure.Repositories.A2.SendEmails;


public class SendEmailLogRepository : ISendEmailLogRepository
{
    private readonly IMasterDataDbContext _db;
    public SendEmailLogRepository(IMasterDataDbContext biDbContext)
    {
        _db = biDbContext;
    }
    public async Task<Result<List<A2_SendEmailLog>>> Get(string sendEmailId)
    {
        try
        {
            var _data = await (from _do in _db.A2_SendEmailLog
                               where _do.SendEmailId == sendEmailId
                               select _do).ToListAsync();

            return new Result<List<A2_SendEmailLog>>(_data, "Thành công!", true);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    public async Task<Result<List<A2_SendEmailLog>>> GetAlls(ScheduleSendEmailLogModel request)
    {
        try
        {
            var _data = await (from _do in _db.A2_SendEmailLog
                               where
                                (request.StartDate != null ? _do.TimeSent.Value.Date >= request.StartDate.Value.Date : true)
                                && (request.EndDate != null ? _do.TimeSent.Value.Date <= request.EndDate.Value.Date : true)
                               orderby _do.TimeSent descending
                               select _do).ToListAsync();

            return new Result<List<A2_SendEmailLog>>(_data, "Xóa thành công!", true);
        }
        catch (Exception ex)
        {
            return new Result<List<A2_SendEmailLog>>("Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<Result<A2_SendEmailLog>> UpdateAsync(A2_SendEmailLog data)
    {
        string message = "";
        try
        {
            var _order = _db.A2_SendEmailLog.FirstOrDefault(o => o.Id == data.Id);
            if (_order != null)
            {
                data.CopyPropertiesTo(_order);
                _db.A2_SendEmailLog.Update(_order);
                message = "Cập nhật thành công";
            }
            else
            {
                _order = new A2_SendEmailLog();
                data.CopyPropertiesTo(_order);
                _db.A2_SendEmailLog.Add(_order);
                message = "Thêm mới thành công";
            }
            var retVal = await _db.SaveChangesAsync();
            return new Result<A2_SendEmailLog>(data, message, true);
        }
        catch (Exception ex)
        {
            return new Result<A2_SendEmailLog>(data, "Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<Result<int>> DeleteAsync(string id)
    {
        try
        {
            var result = _db.A2_SendEmailLog.FirstOrDefault(o => o.Id == id);
            if (result == null)
                return new Result<int>("Không tìm thấy dữ liệu", false);

            _db.A2_SendEmailLog.Remove(result);
            var retVal = await _db.SaveChangesAsync();

            return new Result<int>(retVal, "Xóa thành công!", true);
        }
        catch (Exception ex)
        {
            return new Result<int>(0, "Lỗi: " + ex.ToString(), false);
        }
    }

}
