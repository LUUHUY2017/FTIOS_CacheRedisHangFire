using AMMS.Shared.Commons;
using Microsoft.EntityFrameworkCore;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.Organizations;
using Server.Core.Interfaces.A2.ScheduleSendEmails.Requests;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.Commons;

namespace Server.Infrastructure.Repositories.A2.Organizations;

public class OrganizationRepository : RepositoryBaseMasterData<Organization>, IOrganizationRepository
{

    private readonly MasterDataDbContext _db;
    public OrganizationRepository(MasterDataDbContext dbContext) : base(dbContext)
    {
        _db = dbContext;
    }

    public async Task<Result<Organization>> ActiveAsync(ActiveRequest data)
    {
        string message = "";
        try
        {
            var _order = _db.Organization.FirstOrDefault(o => o.Id == data.Id);
            if (_order != null)
            {
                _order.Actived = true;
                _db.Organization.Update(_order);
                message = "Cập nhật thành công";
            }
            var retVal = await _db.SaveChangesAsync();
            return new Result<Organization>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<Organization>("Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<Result<Organization>> InActiveAsync(InactiveRequest data)
    {
        string message = "";
        try
        {
            var _order = _db.Organization.FirstOrDefault(o => o.Id == data.Id);
            if (_order != null)
            {
                _order.Actived = false;
                _db.Organization.Update(_order);
                message = "Cập nhật thành công";
            }
            var retVal = await _db.SaveChangesAsync();
            return new Result<Organization>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<Organization>("Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<Result<List<Organization>>> GetAlls(OrganizationFilterRequest req)
    {
        try
        {
            bool active = req.Actived == "1";
            var _data = await (from _do in _db.Organization
                               where _do.Actived == active
                               && (!string.IsNullOrWhiteSpace(req.Key) && req.ColumnTable == "OrganizationName" ? _do.OrganizationName.Contains(req.Key.Trim()) : true)
                               && (!string.IsNullOrWhiteSpace(req.Key) && req.ColumnTable == "OrganizationCode" ? _do.OrganizationCode.Contains(req.Key.Trim()) : true)
                               && (!string.IsNullOrWhiteSpace(req.Key) && req.ColumnTable == "OrganizationDescription" ? _do.OrganizationDescription.Contains(req.Key.Trim()) : true)
                               select _do).ToListAsync();

            return new Result<List<Organization>>(_data, "Thành công!", true);
        }
        catch (Exception ex)
        {
            return new Result<List<Organization>>("Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<Result<Organization>> GetById(string id)
    {
        string message = "";
        try
        {
            var _order = _db.Organization.FirstOrDefault(o => o.Id == id);
            return new Result<Organization>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<Organization>("Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<Result<Organization>> UpdateAsync(Organization data)
    {
        string message = "";
        try
        {
            var _order = _db.Organization.FirstOrDefault(o => o.Id == data.Id);
            if (_order != null)
            {
                // data.CopyPropertiesTo(_order);
                _order.OrganizationCode = data.OrganizationCode;
                _order.OrganizationName = data.OrganizationName;
                _order.OrganizationDescription = data.OrganizationDescription;
                _db.Organization.Update(_order);
                message = "Cập nhật thành công";
            }
            else
            {
                _order = new Organization();
                data.CopyPropertiesTo(_order);
                _db.Organization.Add(_order);
                message = "Thêm mới thành công";
            }

            var retVal = await _db.SaveChangesAsync();
            return new Result<Organization>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<Organization>(data, "Lỗi: " + ex.ToString(), false);
        }
    }

}


