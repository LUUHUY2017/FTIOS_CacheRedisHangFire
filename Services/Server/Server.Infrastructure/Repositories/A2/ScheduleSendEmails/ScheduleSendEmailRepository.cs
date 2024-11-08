using AMMS.Shared.Commons;
using Microsoft.EntityFrameworkCore;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.ScheduleSendEmails;
using Server.Core.Interfaces.A2.ScheduleSendEmails.Requests;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.Commons;

namespace Server.Infrastructure.Repositories.A2.ScheduleSendEmails;

public class ScheduleSendEmailRepository : IScheduleSendMailRepository
{
    private readonly IMasterDataDbContext _db;
    public ScheduleSendEmailRepository(IMasterDataDbContext biDbContext)
    {
        _db = biDbContext;
    }

    public async Task<Result<ScheduleSendMail>> ActiveAsync(ActiveRequest data)
    {
        string message = "";
        try
        {
            var _order = _db.ScheduleSendMail.FirstOrDefault(o => o.Id == data.Id);
            if (_order != null)
            {
                _order.Actived = true;
                _db.ScheduleSendMail.Update(_order);
                message = "Cập nhật thành công";
            }
            var retVal = await _db.SaveChangesAsync();
            return new Result<ScheduleSendMail>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<ScheduleSendMail>("Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<Result<ScheduleSendMail>> InActiveAsync(InactiveRequest data)
    {
        string message = "";
        try
        {
            var _order = _db.ScheduleSendMail.FirstOrDefault(o => o.Id == data.Id);
            if (_order != null)
            {
                _order.Actived = false;
                _db.ScheduleSendMail.Update(_order);
                message = "Cập nhật thành công";
            }
            var retVal = await _db.SaveChangesAsync();
            return new Result<ScheduleSendMail>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<ScheduleSendMail>("Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<List<ScheduleSendMail>> GetAlls(ScheduleSendEmailFilterRequest request)
    {
        try
        {
            bool active = request.Actived == "1";
            var _data = await (from _do in _db.ScheduleSendMail
                               where _do.Actived == active
                               && (request.OrganizationId != "0" ? _do.OrganizationId == request.OrganizationId : true)
                               && (!string.IsNullOrWhiteSpace(request.Key) && request.ColumnTable == "ScheduleName" ? _do.ScheduleName.Contains(request.Key) : true)
                               && (!string.IsNullOrWhiteSpace(request.ScheduleNote) ? _do.ScheduleNote == request.ScheduleNote || _do.ScheduleNote == null : true)
                               select _do).ToListAsync();
            return _data;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    public async Task<Result<ScheduleSendMail>> GetById(string id)
    {
        string message = "";
        try
        {
            var _order = _db.ScheduleSendMail.FirstOrDefault(o => o.Id == id);
            return new Result<ScheduleSendMail>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<ScheduleSendMail>("Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<Result<ScheduleSendMail>> UpdateAsync(ScheduleSendMail data)
    {
        string message = "";
        try
        {
            var _order = _db.ScheduleSendMail.FirstOrDefault(o => o.Id == data.Id);
            if (_order != null)
            {
                data.CopyPropertiesTo(_order);
                _db.ScheduleSendMail.Update(_order);
                message = "Cập nhật thành công";
            }
            else
            {
                _order = new ScheduleSendMail();
                data.CopyPropertiesTo(_order);
                _db.ScheduleSendMail.Add(_order);
                message = "Thêm mới thành công";
            }

            var retVal = await _db.SaveChangesAsync();
            return new Result<ScheduleSendMail>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<ScheduleSendMail>(data, "Lỗi: " + ex.ToString(), false);
        }
    }


}
