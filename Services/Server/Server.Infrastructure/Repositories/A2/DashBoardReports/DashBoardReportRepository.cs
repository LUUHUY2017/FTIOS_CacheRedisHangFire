using AMMS.Shared.Commons;
using Microsoft.EntityFrameworkCore;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.DashBoardReports;
using Server.Core.Interfaces.A2.DashBoardReports.Models;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.Commons;

namespace Server.Infrastructure.Repositories.A2.DashBoardReports;

public class DashBoardReportRepository : IDashBoardReportRepository
{
    private readonly MasterDataDbContext _biDbContext;
    public DashBoardReportRepository(MasterDataDbContext dbContext)
    {
        _biDbContext = dbContext;
    }

    public async Task<Result<ScheduleSendMail>> ActiveAsync(ActiveRequest data)
    {
        string message = "";
        try
        {
            var _order = _biDbContext.ScheduleSendMail.FirstOrDefault(o => o.Id == data.Id);
            if (_order != null)
            {
                _order.Actived = true;
                _biDbContext.ScheduleSendMail.Update(_order);
                message = "Cập nhật thành công";
            }
            var retVal = await _biDbContext.SaveChangesAsync();
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
            var _order = _biDbContext.ScheduleSendMail.FirstOrDefault(o => o.Id == data.Id);
            if (_order != null)
            {
                _order.Actived = false;
                _biDbContext.ScheduleSendMail.Update(_order);
                message = "Cập nhật thành công";
            }
            var retVal = await _biDbContext.SaveChangesAsync();
            return new Result<ScheduleSendMail>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<ScheduleSendMail>("Lỗi: " + ex.ToString(), false);
        }
    }

    public Task<List<ScheduleSendMail>> GetAlls(DashBoardReportModel request)
    {
        try
        {
            bool active = request.Actived == "1";
            var _data = (from _do in _biDbContext.ScheduleSendMail
                         where _do.Actived == active
                         && _do.ScheduleNote == request.Note
                         //&& ((request.OrganizationId != "0" || !string.IsNullOrEmpty(request.OrganizationId)) ? _do.OrganizationId == request.OrganizationId : true)
                         && ((!string.IsNullOrEmpty(request.ColumnTable) && request.ColumnTable == "ScheduleName") ? _do.ScheduleName.Contains(request.Key.Trim()) : true)
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
            var _order = _biDbContext.ScheduleSendMail.FirstOrDefault(o => o.Id == id);
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
            var _order = _biDbContext.ScheduleSendMail.FirstOrDefault(o => o.Id == data.Id);
            if (_order != null)
            {
                data.CopyPropertiesTo(_order);
                _biDbContext.ScheduleSendMail.Update(_order);
                message = "Cập nhật thành công";
            }
            else
            {
                _order = new ScheduleSendMail();
                data.CopyPropertiesTo(_order);
                _biDbContext.ScheduleSendMail.Add(_order);
                message = "Thêm mới thành công";
            }

            var retVal = await _biDbContext.SaveChangesAsync();
            return new Result<ScheduleSendMail>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<ScheduleSendMail>(data, "Lỗi: " + ex.ToString(), false);
        }
    }
}
