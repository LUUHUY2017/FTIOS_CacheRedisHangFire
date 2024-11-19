using AMMS.Shared.Commons;
using Microsoft.EntityFrameworkCore;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.StudentClassRoomYears;
using Server.Core.Interfaces.A2.StudentClassRoomYears.Reponses;
using Server.Core.Interfaces.A2.StudentClassRoomYears.Requests;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.Commons;

namespace Server.Infrastructure.Repositories.A2.StudentClassRoomYears;

public class StudentClassRoomYearRepository : RepositoryBaseMasterData<StudentClassRoomYear>, IStudentClassRoomYearRepository
{
    private readonly IMasterDataDbContext _db;
    public StudentClassRoomYearRepository(MasterDataDbContext dbContext) : base(dbContext)
    {
        _db = dbContext;
    }

    public string UserId { get; set; }
    public async Task<Result<StudentClassRoomYear>> SaveAsync(StudentClassRoomYear data)
    {
        string message = "";
        try
        {
            var _order = await _dbContext.StudentClassRoomYear.FirstOrDefaultAsync(o => o.Id == data.Id);
            if (_order != null)
            {
                data.CopyPropertiesTo(_order);
                _dbContext.StudentClassRoomYear.Update(_order);
                message = "Cập nhật thành công";
            }
            else
            {
                _order = new StudentClassRoomYear();
                data.CopyPropertiesTo(_order);
                await _dbContext.StudentClassRoomYear.AddAsync(_order);
                message = "Thêm mới thành công";
            }
            var retVal = await _dbContext.SaveChangesAsync();

            return new Result<StudentClassRoomYear>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<StudentClassRoomYear>(data, "Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<Result<StudentClassRoomYear>> SaveDataAsync(StudentClassRoomYear data)
    {
        string message = "";
        try
        {
            var _order = await _dbContext.StudentClassRoomYear.FirstOrDefaultAsync(o => o.SchoolId == data.SchoolId && o.ClassRoomId == data.ClassRoomId && o.SchoolYearId == data.SchoolYearId);
            if (_order != null)
            {
                _order.ReferenceId = data.Id;
                _order.LastModifiedDate = DateTime.Now;

                _order.Name = data.Name;

                _order.SchoolId = data.SchoolId;
                _order.ClassRoomId = data.ClassRoomId;
                _order.SchoolYearId = data.SchoolYearId;
                _order.OrganizationId = data.OrganizationId;

                _dbContext.StudentClassRoomYear.Update(_order);
                message = "Cập nhật thành công";
            }
            else
            {
                _order = new StudentClassRoomYear();
                data.CopyPropertiesTo(_order);
                _order.Id = !string.IsNullOrWhiteSpace(data.Id) ? Guid.NewGuid().ToString() : data.Id;
                await _dbContext.StudentClassRoomYear.AddAsync(_order);
                message = "Thêm mới thành công";
            }
            var retVal = await _dbContext.SaveChangesAsync();

            return new Result<StudentClassRoomYear>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<StudentClassRoomYear>(data, "Lỗi: " + ex.ToString(), false);
        }
    }

    public async Task<Result<StudentClassRoomYear>> ActiveAsync(ActiveRequest data)
    {
        string message = "";
        try
        {
            var _order = _db.StudentClassRoomYear.FirstOrDefault(o => o.Id == data.Id);
            if (_order != null)
            {
                _order.Actived = true;
                _db.StudentClassRoomYear.Update(_order);
                message = "Cập nhật thành công";
            }
            var retVal = await _db.SaveChangesAsync();
            return new Result<StudentClassRoomYear>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<StudentClassRoomYear>("Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<Result<StudentClassRoomYear>> InActiveAsync(InactiveRequest data)
    {
        string message = "";
        try
        {
            var _order = _db.StudentClassRoomYear.FirstOrDefault(o => o.Id == data.Id);
            if (_order != null)
            {
                _order.Actived = false;
                _db.StudentClassRoomYear.Update(_order);
                message = "Cập nhật thành công";
            }
            var retVal = await _db.SaveChangesAsync();
            return new Result<StudentClassRoomYear>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<StudentClassRoomYear>("Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<List<ClassRoomYearReportResponse>> GetAlls(ClassRoomYearFilterRequest request)
    {
        try
        {
            bool active = request.Actived == "1";
            var _data = await (from _do in _db.StudentClassRoomYear
                               join _or in _db.Organization on _do.OrganizationId equals _or.Id into OT
                               from or in OT.DefaultIfEmpty()
                               where _do.Actived == active
                                 && ((!string.IsNullOrWhiteSpace(request.OrganizationId) && request.OrganizationId != "0") ? _do.OrganizationId == request.OrganizationId : true)
                               && (!string.IsNullOrWhiteSpace(request.Key) && request.ColumnTable == "Name" ? _do.Name.Contains(request.Key) : true)
                               select new ClassRoomYearReportResponse()
                               {
                                   Id = _do.Id,
                                   CreatedBy = _do.CreatedBy,
                                   Actived = _do.Actived,
                                   CreatedDate = _do.CreatedDate,
                                   LastModifiedDate = _do.LastModifiedDate != null ? _do.LastModifiedDate : _do.CreatedDate,
                                   OrganizationId = _do.OrganizationId,
                                   Name = _do.Name,
                                   OrganizationName = or != null ? or.OrganizationName : null,

                               }).OrderBy(o => o.Name).ToListAsync();
            return _data;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    public async Task<List<StudentClassRoomYear>> Gets(bool actived = true)
    {
        try
        {
            var _data = await (from _do in _db.StudentClassRoomYear where _do.Actived == actived select _do).ToListAsync();
            return _data;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    public async Task<Result<StudentClassRoomYear>> GetById(string id)
    {
        string message = "";
        try
        {
            var _order = _db.StudentClassRoomYear.FirstOrDefault(o => o.Id == id);
            return new Result<StudentClassRoomYear>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<StudentClassRoomYear>("Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<Result<StudentClassRoomYear>> UpdateAsync(StudentClassRoomYear data)
    {
        string message = "";
        try
        {
            var _order = _db.StudentClassRoomYear.FirstOrDefault(o => o.Id == data.Id);
            if (_order != null)
            {
                data.CopyPropertiesTo(_order);
                _db.StudentClassRoomYear.Update(_order);
                message = "Cập nhật thành công";
            }
            else
            {
                _order = new StudentClassRoomYear();
                data.CopyPropertiesTo(_order);
                _db.StudentClassRoomYear.Add(_order);
                message = "Thêm mới thành công";
            }

            var retVal = await _db.SaveChangesAsync();
            return new Result<StudentClassRoomYear>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<StudentClassRoomYear>(data, "Lỗi: " + ex.ToString(), false);
        }
    }
}
