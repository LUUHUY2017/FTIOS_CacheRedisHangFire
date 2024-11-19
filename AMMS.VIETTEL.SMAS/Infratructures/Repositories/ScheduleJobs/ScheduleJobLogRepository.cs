using AMMS.Shared.Commons;
using AMMS.VIETTEL.SMAS.Cores.Entities.A2;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.ScheduleJobs;
using AMMS.VIETTEL.SMAS.Infratructures.Databases;
using Microsoft.EntityFrameworkCore;
using Shared.Core.Commons;

namespace AMMS.VIETTEL.SMAS.Infratructures.Repositories.ScheduleJobs;

public class ScheduleJobLogRepository : IScheduleJobLogRepository
{
    private readonly ViettelDbContext _db;
    public ScheduleJobLogRepository(ViettelDbContext biDbContext)
    {
        _db = biDbContext;
    }

    public async Task<Result<ScheduleJobLog>> ActiveAsync(ActiveRequest data)
    {
        string message = "";
        try
        {
            var _order = _db.ScheduleJobLog.FirstOrDefault(o => o.Id == data.Id);
            if (_order != null)
            {
                _order.Actived = true;
                _db.ScheduleJobLog.Update(_order);
                message = "Cập nhật thành công";
            }
            var retVal = await _db.SaveChangesAsync();
            return new Result<ScheduleJobLog>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<ScheduleJobLog>("Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<Result<ScheduleJobLog>> InActiveAsync(InactiveRequest data)
    {
        string message = "";
        try
        {
            var _order = _db.ScheduleJobLog.FirstOrDefault(o => o.Id == data.Id);
            if (_order != null)
            {
                _order.Actived = false;
                _db.ScheduleJobLog.Update(_order);
                message = "Cập nhật thành công";
            }
            var retVal = await _db.SaveChangesAsync();
            return new Result<ScheduleJobLog>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<ScheduleJobLog>("Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<List<ScheduleJobLog>> GetByScheduleJobId(string id)
    {
        try
        {
            var _data = await (from _do in _db.ScheduleJobLog
                               where _do.Actived == true && _do.ScheduleJobId == id
                               select _do).Take(15).OrderByDescending(o => o.CreatedDate).ToListAsync();
            return _data;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    public async Task<List<ScheduleJobLog>> Gets(bool actived = true)
    {
        try
        {
            var _data = await (from _do in _db.ScheduleJobLog where _do.Actived == actived select _do).ToListAsync();
            return _data;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    public async Task<Result<ScheduleJobLog>> GetById(string id)
    {
        string message = "";
        try
        {
            var _order = _db.ScheduleJobLog.FirstOrDefault(o => o.Id == id);
            return new Result<ScheduleJobLog>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<ScheduleJobLog>("Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<Result<ScheduleJobLog>> UpdateAsync(ScheduleJobLog data)
    {
        string message = "";
        try
        {
            var _order = _db.ScheduleJobLog.FirstOrDefault(o => o.Id == data.Id);
            if (_order != null)
            {
                data.CopyPropertiesTo(_order);
                _db.ScheduleJobLog.Update(_order);
                message = "Cập nhật thành công";
            }
            else
            {
                _order = new ScheduleJobLog();
                data.CopyPropertiesTo(_order);
                _db.ScheduleJobLog.Add(_order);
                message = "Thêm mới thành công";
            }

            var retVal = await _db.SaveChangesAsync();
            return new Result<ScheduleJobLog>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<ScheduleJobLog>(data, "Lỗi: " + ex.ToString(), false);
        }
    }


}
