using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Shared.Core.Commons;
using Shared.Core.Entities;
using Shared.Core.Repositories;

namespace AMMS.VIETTEL.SMAS.Infratructures.Databases;

public class RepositoryBase<T> : IAsyncRepository<T> where T : EntityBase
{
    protected readonly ViettelDbContext _dbContext;

    public string UserId { get; set; } = "";

    public RepositoryBase(ViettelDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<List<T>> GetAllAsync()
    {
        return await _dbContext.Set<T>().ToListAsync();
    }

    public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbContext.Set<T>().Where(predicate).ToListAsync();
    }

    public async Task<Result<T?>> GetByFirstAsync(Expression<Func<T, bool>> predicate)
    {
        var entity = await _dbContext.Set<T>().Where(predicate).FirstOrDefaultAsync();
        if (entity != null)
        {
            return new Result<T>()
            {
                Data = entity,
                Code = 0,
                Message = "Thành công",
                Succeeded = true
            };
        }
        return new Result<T>()
        {
            Data = null,
            Code = 0,
            Message = "Thành công",
            Succeeded = false
        };
    }
    public async Task<Result<T>> GetByIdAsync(string id)
    {
        var entity = await _dbContext.Set<T>().FindAsync(id)!;
        return new Result<T>()
        {
            Data = entity,
            Code = 0,
            Message = "Thành công",
            Succeeded = true
        };
    }

    public async Task<Result<T>> AddAsync(T entity)
    {
        entity.LastModifiedBy = entity.CreatedBy = UserId;
        entity.LastModifiedDate = entity.CreatedDate = DateTime.Now;

        //string logs = $"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")} User {UserId} Add {JsonConvert.SerializeObject(entity)}";
        //if (string.IsNullOrEmpty(entity.Logs))
        //    entity.Logs = logs;
        //else
        //    entity.Logs += "; " + logs;

        entity.Logs = "";
        try
        {
            var t = await _dbContext.Set<T>().AddAsync(entity);
            var retVal = await _dbContext.SaveChangesAsync();
            if (retVal > 0)
                return new Result<T>()
                {
                    Data = entity,
                    Succeeded = true,
                    Message = "Thêm mới dữ liệu thành công!",
                };
            return new Result<T>()
            {
                Data = entity,
                Succeeded = false,
                Message = "Thêm mới dữ liệu không thành công",
            };
        }
        catch (Exception e)
        {
            return new Result<T>()
            {
                Data = entity,
                Succeeded = false,
                Message = "Thêm mới dữ liệu không thành công",
                Exception = e,
            };
        }


    }
    public async Task<Result<List<T>>> AddRangeAsync(List<T> entities)
    {
        foreach (var entity in entities)
        {
            entity.CreatedBy = UserId;
            entity.CreatedDate = DateTime.Now;

            entity.LastModifiedBy = UserId;
            entity.LastModifiedDate = DateTime.Now;
        }
        await _dbContext.Set<T>().AddRangeAsync(entities);


        var retVal = await _dbContext.SaveChangesAsync();
        if (retVal > 0)
            return new Result<List<T>>()
            {
                Data = entities,
                Succeeded = true,
                Message = "Cập nhật dữ liệu thành công!",
            };
        return new Result<List<T>>()
        {
            Data = entities,
            Succeeded = false,
            Message = "Cập nhật dữ liệu không thành công",
        };
    }
    async Task<Result<T>> Update(T entity)
    {
        try
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            var retval = await _dbContext.SaveChangesAsync();
            if (retval > 0)
                return new Result<T>(entity, "Cập nhật thành công!", true);
            return new Result<T>(entity, "Cập nhật không thành công.", false);
        }
        catch (Exception e)
        {
            return new Result<T>(entity, $"Cập nhật lỗi: {e.Message}", false);
        }

    }
    public async Task<Result<T>> UpdateAsync(T entity)
    {
        entity.LastModifiedBy = UserId;
        entity.LastModifiedDate = DateTime.Now;

        //string logs = $"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")} User {UserId} Update {JsonConvert.SerializeObject(entity)}";
        //if (string.IsNullOrEmpty(entity.Logs))
        //    entity.Logs = logs;
        //else
        //    entity.Logs += "; " + logs;

        entity.Logs = "";

        return await Update(entity);
    }

    public async Task<Result<T>> ActiveAsync(ActiveRequest request)
    {
        var result = await GetByIdAsync(request.Id);
        if (!result.Succeeded)
        {
            return new Result<T>("Không tìm thấy dữ liệu", false);
        }
        var entity = result.Data;
        entity.Actived = true;
        entity.Reason = request.Reason;

        entity.LastModifiedBy = UserId;
        entity.LastModifiedDate = DateTime.Now;

        //string logs = $"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")} User {UserId} Active {JsonConvert.SerializeObject(entity)}";
        //if (string.IsNullOrEmpty(entity.Logs))
        //    entity.Logs = logs;
        //else
        //    entity.Logs += "; " + logs;

        entity.Logs = "";

        return await Update(entity);
    }

    public async Task<Result<T>> InactiveAsync(InactiveRequest request)
    {
        var result = await GetByIdAsync(request.Id);
        if (!result.Succeeded)
        {
            return new Result<T>("Không tìm thấy dữ liệu", false);
        }
        var entity = result.Data;
        entity.Actived = false;
        entity.Reason = request.Reason;

        entity.LastModifiedBy = UserId;
        entity.LastModifiedDate = DateTime.Now;

        //string logs = $"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")} User {UserId} Inactive {JsonConvert.SerializeObject(entity)}";
        //if (string.IsNullOrEmpty(entity.Logs))
        //    entity.Logs = logs;
        //else
        //    entity.Logs += "; " + logs;

        entity.Logs = "";

        return await Update(entity);
    }

    public async Task<Result<int>> DeleteAsync(DeleteRequest request)
    {
        var result = await GetByIdAsync(request.Id);
        if (!result.Succeeded)
        {
            return new Result<int>("Không tìm thấy dữ liệu", false);
        }
        var entity = result.Data;

        _dbContext.Set<T>().Remove(entity);
        var retVal = await _dbContext.SaveChangesAsync();
        if (retVal > 0)
            return new Result<int>(retVal, "Xóa thành công!", true);
        return new Result<int>(retVal, "Xóa không thành công.", false);
    }

    public async Task<Result<List<T>>> UpdateRangeAsync(List<T> entities)
    {
        foreach (var entity in entities)
        {
            var data = await _dbContext.Set<T>().FindAsync(entity.Id)!;
            if (data == null)
            {
                entity.CreatedBy = UserId;
                entity.CreatedDate = DateTime.Now;
                _dbContext.Set<T>().Add(entity);
            }
            else
            {
                entity.LastModifiedBy = UserId;
                entity.LastModifiedDate = DateTime.Now;
                _dbContext.Set<T>().Update(entity);
            }
        }
        var retVal = await _dbContext.SaveChangesAsync();
        if (retVal > 0)
            return new Result<List<T>>()
            {
                Data = entities,
                Succeeded = true,
                Message = "Cập nhật dữ liệu thành công!",
            };
        return new Result<List<T>>()
        {
            Data = entities,
            Succeeded = false,
            Message = "Cập nhật dữ liệu không thành công",
        };
    }
}
