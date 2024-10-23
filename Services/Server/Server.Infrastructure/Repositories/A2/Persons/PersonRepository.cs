using AMMS.Shared.Commons;
using Microsoft.EntityFrameworkCore;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.Persons;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.Commons;
using Shared.Core.Identity.Object;
using System.Linq.Expressions;

namespace Server.Infrastructure.Repositories.A2.Persons;

public class PersonRepository : IPersonRepository
{
    private readonly IMasterDataDbContext _db;

    public string UserId { get; set; }

    public PersonRepository(IMasterDataDbContext biDbContext)
    {
        _db = biDbContext;
    }

    public async Task<Result<A2_Person>> ActiveAsync(ActiveRequest data)
    {
        string message = "";
        try
        {
            var _order = _db.A2_Person.FirstOrDefault(o => o.Id == data.Id);
            if (_order != null)
            {
                _order.Actived = true;
                _db.A2_Person.Update(_order);
                message = "Cập nhật thành công";
            }
            var retVal = await _db.SaveChangesAsync();
            return new Result<A2_Person>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<A2_Person>("Lỗi: " + ex.ToString(), false);
        }
    }

    public async Task<Result<A2_Person>> UpdateAsync(A2_Person data)
    {
        string message = "";
        try
        {
            var _order = await _db.A2_Person.SingleOrDefaultAsync(o => o.Id == data.Id); // Use async version of query
            if (_order != null)
            {
                data.CopyPropertiesTo(_order);
                _db.A2_Person.Update(_order);
                message = "Cập nhật thành công";
            }
            else
            {
                _order = new A2_Person();
                data.CopyPropertiesTo(_order);
                await _db.A2_Person.AddAsync(_order);
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

            return new Result<A2_Person>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<A2_Person>(data, "Lỗi: " + ex.ToString(), false);
        }
    }

    public Task<List<A2_Person>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<A2_Person>> GetAllAsync(Expression<Func<A2_Person, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<Result<A2_Person?>> GetByIdAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<Result<A2_Person?>> GetByFirstAsync(Expression<Func<A2_Person, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public Task<Result<A2_Person>> AddAsync(A2_Person entity)
    {
        throw new NotImplementedException();
    }

    public Task<Result<List<A2_Person>>> AddRangeAsync(List<A2_Person> entities)
    {
        throw new NotImplementedException();
    }

    public Task<Result<List<A2_Person>>> UpdateRangeAsync(List<A2_Person> entities)
    {
        throw new NotImplementedException();
    }

    public Task<Result<int>> DeleteAsync(DeleteRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<Result<A2_Person>> InactiveAsync(InactiveRequest request)
    {
        throw new NotImplementedException();
    }
}
