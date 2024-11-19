using AMMS.Shared.Commons;
using Microsoft.EntityFrameworkCore;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.ClassRooms;
using Server.Core.Interfaces.A2.ClassRooms.Reponses;
using Server.Core.Interfaces.A2.ClassRooms.Requests;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.Commons;

namespace Server.Infrastructure.Repositories.A2.ClassRooms;

public class ClassRoomRepository : RepositoryBaseMasterData<ClassRoom>, IClassRoomRepository
{
    private readonly IMasterDataDbContext _db;
    public ClassRoomRepository(MasterDataDbContext dbContext) : base(dbContext)
    {
        _db = dbContext;
    }

    public string UserId { get; set; }
    public async Task<Result<ClassRoom>> SaveAsync(ClassRoom data)
    {
        string message = "";
        try
        {
            var _order = await _dbContext.ClassRoom.FirstOrDefaultAsync(o => o.Id == data.Id);
            if (_order != null)
            {
                data.CopyPropertiesTo(_order);
                _dbContext.ClassRoom.Update(_order);
                message = "Cập nhật thành công";
            }
            else
            {
                _order = new ClassRoom();
                data.CopyPropertiesTo(_order);

                await _dbContext.ClassRoom.AddAsync(_order);
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

            return new Result<ClassRoom>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<ClassRoom>(data, "Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<Result<ClassRoom>> SaveDataAsync(ClassRoom data)
    {
        string message = "";
        try
        {
            var _order = await _dbContext.ClassRoom.FirstOrDefaultAsync(o => o.SchoolId == data.SchoolId && o.Name == data.Name);
            if (_order != null)
            {
                _order.ReferenceId = data.Id;
                _order.LastModifiedDate = DateTime.Now;
                _order.Name = data.Name;
                _order.OrganizationId = data.OrganizationId;
                _order.SchoolId = data.SchoolId;
                _dbContext.ClassRoom.Update(_order);
                message = "Cập nhật thành công";
            }
            else
            {
                _order = new ClassRoom();
                data.CopyPropertiesTo(_order);
                _order.Id = !string.IsNullOrWhiteSpace(data.Id) ? Guid.NewGuid().ToString() : data.Id;
                await _dbContext.ClassRoom.AddAsync(_order);
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

            return new Result<ClassRoom>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<ClassRoom>(data, "Lỗi: " + ex.ToString(), false);
        }
    }

    public async Task<Result<ClassRoom>> ActiveAsync(ActiveRequest data)
    {
        string message = "";
        try
        {
            var _order = _db.ClassRoom.FirstOrDefault(o => o.Id == data.Id);
            if (_order != null)
            {
                _order.Actived = true;
                _db.ClassRoom.Update(_order);
                message = "Cập nhật thành công";
            }
            var retVal = await _db.SaveChangesAsync();
            return new Result<ClassRoom>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<ClassRoom>("Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<Result<ClassRoom>> InActiveAsync(InactiveRequest data)
    {
        string message = "";
        try
        {
            var _order = _db.ClassRoom.FirstOrDefault(o => o.Id == data.Id);
            if (_order != null)
            {
                _order.Actived = false;
                _db.ClassRoom.Update(_order);
                message = "Cập nhật thành công";
            }
            var retVal = await _db.SaveChangesAsync();
            return new Result<ClassRoom>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<ClassRoom>("Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<List<ClassRoomReportResponse>> GetAlls(ClassRoomFilterRequest request)
    {
        try
        {
            bool active = request.Actived == "1";
            var _data = await (from _do in _db.ClassRoom
                               join _or in _db.Organization on _do.OrganizationId equals _or.Id into OT
                               from or in OT.DefaultIfEmpty()
                               where _do.Actived == active
                                 && ((!string.IsNullOrWhiteSpace(request.OrganizationId) && request.OrganizationId != "0") ? _do.OrganizationId == request.OrganizationId : true)
                               && (!string.IsNullOrWhiteSpace(request.Key) && request.ColumnTable == "Name" ? _do.Name.Contains(request.Key) : true)
                               select new ClassRoomReportResponse()
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
    public async Task<List<ClassRoom>> Gets(bool actived = true)
    {
        try
        {
            var _data = await (from _do in _db.ClassRoom where _do.Actived == actived select _do).ToListAsync();
            return _data;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    public async Task<Result<ClassRoom>> GetById(string id)
    {
        string message = "";
        try
        {
            var _order = _db.ClassRoom.FirstOrDefault(o => o.Id == id);
            return new Result<ClassRoom>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<ClassRoom>("Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<Result<ClassRoom>> UpdateAsync(ClassRoom data)
    {
        string message = "";
        try
        {
            var _order = _db.ClassRoom.FirstOrDefault(o => o.Id == data.Id);
            if (_order != null)
            {
                data.CopyPropertiesTo(_order);
                _db.ClassRoom.Update(_order);
                message = "Cập nhật thành công";
            }
            else
            {
                _order = new ClassRoom();
                data.CopyPropertiesTo(_order);
                _db.ClassRoom.Add(_order);
                message = "Thêm mới thành công";
            }

            var retVal = await _db.SaveChangesAsync();
            return new Result<ClassRoom>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<ClassRoom>(data, "Lỗi: " + ex.ToString(), false);
        }
    }
}
