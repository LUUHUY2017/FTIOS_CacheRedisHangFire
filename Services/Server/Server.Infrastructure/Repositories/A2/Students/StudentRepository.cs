using AMMS.Shared.Commons;
using Microsoft.EntityFrameworkCore;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.Students;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.Commons;
using Shared.Core.Identity.Object;

namespace Server.Infrastructure.Repositories.A2.Students;

public class StudentRepository : IStudentRepository
{
    private readonly IMasterDataDbContext _db;
    public StudentRepository(IMasterDataDbContext biDbContext)
    {
        _db = biDbContext;
    }

    public async Task<Result<A2_Student>> ActiveAsync(ActiveRequest data)
    {
        string message = "";
        try
        {
            var _order = _db.A2_Student.FirstOrDefault(o => o.Id == data.Id);
            if (_order != null)
            {
                _order.Actived = true;
                _db.A2_Student.Update(_order);
                message = "Cập nhật thành công";
            }
            var retVal = await _db.SaveChangesAsync();
            return new Result<A2_Student>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<A2_Student>("Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<Result<A2_Student>> InActiveAsync(InactiveRequest data)
    {
        string message = "";
        try
        {
            var _order = _db.A2_Student.FirstOrDefault(o => o.Id == data.Id);
            if (_order != null)
            {
                _order.Actived = false;
                _db.A2_Student.Update(_order);
                message = "Cập nhật thành công";
            }
            var retVal = await _db.SaveChangesAsync();
            return new Result<A2_Student>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<A2_Student>("Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<List<A2_Student>> GetAll()
    {
        try
        {
            var _data = await (from o in _db.A2_Student
                               where o.Actived == true
                               select o).ToListAsync();
            return _data;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    public async Task<Result<A2_Student>> GetById(string id)
    {
        string message = "";
        try
        {
            var _order = _db.A2_Student.FirstOrDefault(o => o.Id == id);
            return new Result<A2_Student>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<A2_Student>("Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<Result<A2_Student>> UpdateAsync(A2_Student data)
    {
        string message = "";
        try
        {
            var _order = await _db.A2_Student.SingleOrDefaultAsync(o => o.Id == data.Id); // Use async version of query
            if (_order != null)
            {
                data.CopyPropertiesTo(_order);
                _db.A2_Student.Update(_order);
                message = "Cập nhật thành công";
            }
            else
            {
                _order = new A2_Student();
                data.CopyPropertiesTo(_order);
                await _db.A2_Student.AddAsync(_order);
                message = "Thêm mới thành công";
            }

            try
            {
                var retVal = await _db.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                // Log and handle the exception, if necessary
                message = $"Error occurred: {ex.Message}";
            }

            return new Result<A2_Student>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<A2_Student>(data, "Lỗi: " + ex.ToString(), false);
        }
    }

}
