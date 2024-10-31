using AMMS.Shared.Commons;
using Microsoft.EntityFrameworkCore;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.Organizations;
using Server.Core.Interfaces.A2.ScheduleSendEmails.Requests;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.Commons;

namespace Server.Infrastructure.Repositories.A2.Organizations;

public class OrganizationRepository : RepositoryBaseMasterData<A2_Organization>, IOrganizationRepository
{

    private readonly MasterDataDbContext _db;
    public OrganizationRepository(MasterDataDbContext dbContext) : base(dbContext)
    {
        _db = dbContext;
    }

    public async Task<Result<A2_Organization>> ActiveAsync(ActiveRequest data)
    {
        string message = "";
        try
        {
            var _order = _db.A2_Organization.FirstOrDefault(o => o.Id == data.Id);
            if (_order != null)
            {
                _order.Actived = true;
                _db.A2_Organization.Update(_order);
                message = "Cập nhật thành công";
            }
            var retVal = await _db.SaveChangesAsync();
            return new Result<A2_Organization>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<A2_Organization>("Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<Result<A2_Organization>> InActiveAsync(InactiveRequest data)
    {
        string message = "";
        try
        {
            var _order = _db.A2_Organization.FirstOrDefault(o => o.Id == data.Id);
            if (_order != null)
            {
                _order.Actived = false;
                _db.A2_Organization.Update(_order);
                message = "Cập nhật thành công";
            }
            var retVal = await _db.SaveChangesAsync();
            return new Result<A2_Organization>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<A2_Organization>("Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<Result<List<A2_Organization>>> GetAlls(OrganizationFilterRequest req)
    {
        try
        {
            bool active = req.Actived == "1";
            var _data = await (from _do in _db.A2_Organization
                               where _do.Actived == active
                               && (!string.IsNullOrWhiteSpace(req.Key) && req.ColumnTable == "OrganizationName" ? _do.OrganizationName.Contains(req.Key.Trim()) : true)
                               && (!string.IsNullOrWhiteSpace(req.Key) && req.ColumnTable == "OrganizationCode" ? _do.OrganizationCode.Contains(req.Key.Trim()) : true)
                               && (!string.IsNullOrWhiteSpace(req.Key) && req.ColumnTable == "OrganizationDescription" ? _do.OrganizationDescription.Contains(req.Key.Trim()) : true)
                               select _do).ToListAsync();

            return new Result<List<A2_Organization>>(_data, "Thành công!", true);
        }
        catch (Exception ex)
        {
            return new Result<List<A2_Organization>>("Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<Result<A2_Organization>> GetById(string id)
    {
        string message = "";
        try
        {
            var _order = _db.A2_Organization.FirstOrDefault(o => o.Id == id);
            return new Result<A2_Organization>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<A2_Organization>("Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<Result<A2_Organization>> UpdateAsync(A2_Organization data)
    {
        string message = "";
        try
        {
            var _order = _db.A2_Organization.FirstOrDefault(o => o.Id == data.Id);
            if (_order != null)
            {
                data.CopyPropertiesTo(_order);
                _db.A2_Organization.Update(_order);
                message = "Cập nhật thành công";
            }
            else
            {
                _order = new A2_Organization();
                data.CopyPropertiesTo(_order);
                _db.A2_Organization.Add(_order);
                message = "Thêm mới thành công";
            }

            var retVal = await _db.SaveChangesAsync();
            return new Result<A2_Organization>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<A2_Organization>(data, "Lỗi: " + ex.ToString(), false);
        }
    }

}


