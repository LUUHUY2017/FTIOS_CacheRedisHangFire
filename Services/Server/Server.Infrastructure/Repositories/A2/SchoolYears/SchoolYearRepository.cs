using AMMS.Shared.Commons;
using Microsoft.EntityFrameworkCore;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.SchoolYears;
using Server.Core.Interfaces.A2.SchoolYears.Reponses;
using Server.Core.Interfaces.A2.SchoolYears.Requests;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.Commons;

namespace Server.Infrastructure.Repositories.A2.SchoolYears;

public class SchoolYearRepository : RepositoryBaseMasterData<SchoolYear>, ISchoolYearRepository
{
    private readonly IMasterDataDbContext _db;
    public SchoolYearRepository(MasterDataDbContext dbContext) : base(dbContext)
    {
        _db = dbContext;
    }

    public string UserId { get; set; }
    public async Task<Result<SchoolYear>> SaveAsync(SchoolYear data)
    {
        string message = "";
        try
        {
            var _order = await _dbContext.SchoolYear.FirstOrDefaultAsync(o => o.Id == data.Id);
            if (_order != null)
            {
                data.CopyPropertiesTo(_order);
                _dbContext.SchoolYear.Update(_order);
                message = "Cập nhật thành công";
            }
            else
            {
                _order = new SchoolYear();
                data.CopyPropertiesTo(_order);

                await _dbContext.SchoolYear.AddAsync(_order);
                message = "Thêm mới thành công";
            }

            try
            {
                var retVal = await _dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                // Log and handle the exception, if necessary
                message = $"Error occurred: {ex.Message}";
            }

            return new Result<SchoolYear>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<SchoolYear>(data, "Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<Result<SchoolYear>> SaveDataAsync(SchoolYear data)
    {
        string message = "";
        try
        {
            var _order = await _dbContext.SchoolYear.FirstOrDefaultAsync(o => o.Name == data.Name);
            if (_order != null)
            {
                _order.ReferenceId = data.Id;
                _order.LastModifiedDate = DateTime.Now;
                _order.Name = data.Name;
                _order.Depcription = data.Depcription;
                _order.Start = data.Start;
                _order.End = data.End;
                _order.OrganizationId = data.OrganizationId;

                _dbContext.SchoolYear.Update(_order);
                message = "Cập nhật thành công";
            }
            else
            {
                _order = new SchoolYear();
                data.CopyPropertiesTo(_order);
                _order.Id = !string.IsNullOrWhiteSpace(data.Id) ? Guid.NewGuid().ToString() : data.Id;
                await _dbContext.SchoolYear.AddAsync(_order);
                message = "Thêm mới thành công";
            }

            try
            {
                var retVal = await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                message = $"Error occurred: {ex.Message}";
            }

            return new Result<SchoolYear>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<SchoolYear>(data, "Lỗi: " + ex.ToString(), false);
        }
    }

    public async Task<Result<SchoolYear>> ActiveAsync(ActiveRequest data)
    {
        string message = "";
        try
        {
            var _order = _db.SchoolYear.FirstOrDefault(o => o.Id == data.Id);
            if (_order != null)
            {
                _order.Actived = true;
                _db.SchoolYear.Update(_order);
                message = "Cập nhật thành công";
            }
            var retVal = await _db.SaveChangesAsync();
            return new Result<SchoolYear>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<SchoolYear>("Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<Result<SchoolYear>> InActiveAsync(InactiveRequest data)
    {
        string message = "";
        try
        {
            var _order = _db.SchoolYear.FirstOrDefault(o => o.Id == data.Id);
            if (_order != null)
            {
                _order.Actived = false;
                _db.SchoolYear.Update(_order);
                message = "Cập nhật thành công";
            }
            var retVal = await _db.SaveChangesAsync();
            return new Result<SchoolYear>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<SchoolYear>("Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<List<SchoolYearReportResponse>> GetAlls(SchoolYearFilterRequest request)
    {
        try
        {
            bool active = request.Actived == "1";
            var _data = await (from _do in _db.SchoolYear
                               join _or in _db.Organization on _do.OrganizationId equals _or.Id into OT
                               from or in OT.DefaultIfEmpty()
                               where _do.Actived == active

                               && (!string.IsNullOrWhiteSpace(request.Key) && request.ColumnTable == "Name" ? _do.Name.Contains(request.Key) : true)
                               select new SchoolYearReportResponse()
                               {
                                   Id = _do.Id,
                                   CreatedBy = _do.CreatedBy,
                                   Actived = _do.Actived,
                                   CreatedDate = _do.CreatedDate,
                                   LastModifiedDate = _do.LastModifiedDate != null ? _do.LastModifiedDate : _do.CreatedDate,
                                   OrganizationId = _do.OrganizationId,
                                   Start = _do.Start,
                                   End = _do.End,
                                   Name = _do.Name,
                                   Depcription = _do.Depcription,

                               }).OrderBy(o => o.Name).ToListAsync();
            return _data;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    public async Task<List<SchoolYear>> Gets(bool actived = true)
    {
        try
        {
            var _data = await (from _do in _db.SchoolYear where _do.Actived == actived select _do).ToListAsync();
            return _data;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    public async Task<Result<SchoolYear>> GetById(string id)
    {
        string message = "";
        try
        {
            var _order = _db.SchoolYear.FirstOrDefault(o => o.Id == id);
            return new Result<SchoolYear>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<SchoolYear>("Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<Result<SchoolYear>> UpdateAsync(SchoolYear data)
    {
        string message = "";
        try
        {
            var _order = _db.SchoolYear.FirstOrDefault(o => o.Id == data.Id);
            if (_order != null)
            {
                data.CopyPropertiesTo(_order);
                _db.SchoolYear.Update(_order);
                message = "Cập nhật thành công";
            }
            else
            {
                _order = new SchoolYear();
                data.CopyPropertiesTo(_order);
                _db.SchoolYear.Add(_order);
                message = "Thêm mới thành công";
            }

            var retVal = await _db.SaveChangesAsync();
            return new Result<SchoolYear>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<SchoolYear>(data, "Lỗi: " + ex.ToString(), false);
        }
    }
}
