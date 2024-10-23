using Shared.Core.Commons;
using Shared.Core.Entities;
using System.Linq.Expressions;

namespace AMMS.Core.Repositories;

public interface IAsyncRepository<T> where T : EntityBase
{
    public string UserId { get; set; }
    Task<List<T>> GetAllAsync();
    Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate);
    Task<Result<T?>> GetByIdAsync(string id);
    Task<Result<T?>> GetByFirstAsync(Expression<Func<T, bool>> predicate);

    /// <summary>
    /// Thêm mới bản ghi
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<Result<T>> AddAsync(T entity);
    Task<Result<List<T>>> AddRangeAsync(List<T> entities);
    Task<Result<List<T>>> UpdateRangeAsync(List<T> entities);
    /// <summary>
    /// Cập nhật bản ghi
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<Result<T>> UpdateAsync(T entity);
    /// <summary>
    /// Xóa bản ghi khỏi CSDL
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<Result<int>> DeleteAsync(DeleteRequest request);
    /// <summary>
    /// Đánh dấu sử dụng
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<Result<T>> ActiveAsync(ActiveRequest request);
    /// <summary>
    /// Đánh dấu ngưng sử dụng
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    Task<Result<T>> InactiveAsync(InactiveRequest request);

}