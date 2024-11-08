using System.Linq.Expressions;
using Server.Core.Identity.Entities;
using Shared.Core.Commons;

namespace Server.Core.Identity.Repositories;

public interface IOTPRepository
{
    Task<List<OTP>> GetAllAsync(Expression<Func<OTP, bool>> predicate);

    Task<Result<OTP>> AddAsync(OTP entity);
    Task<Result<OTP>> UpdateAsync(OTP entity);
}
