using Shared.Core.Commons;
using Shared.Core.Entities;
using System.Linq.Expressions;

namespace Shared.Core.Repositories
{
    public interface IAsyncDataInOutRepository<T> where T : EntityDataInOutBase
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>> predicate);
        Task<T?> GetByIdAsync(decimal id);
        Task<T> AddAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<Result<int>> DeleteAsync(T entity);
    }
}
