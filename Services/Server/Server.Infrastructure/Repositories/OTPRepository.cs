using Microsoft.EntityFrameworkCore;
using Server.Core.Identity.Entities;
using Server.Core.Identity.Repositories;
using Server.Core.Interfaces.A2.Persons;
using Server.Infrastructure.Identity;
using Shared.Core.Commons;
using System.Linq.Expressions;

namespace Server.Infrastructure.Repositories;

public class OTPRepository : IOTPRepository
{
    private readonly IdentityContext _identityContext;
    private readonly IPersonRepository _personRepository;

    public OTPRepository(IdentityContext identityContext,
        IPersonRepository personRepository)
    {
        _identityContext = identityContext;
        _personRepository = personRepository;
    }
    public async Task<Result<OTP>> AddAsync(OTP entity)
    {
        await _identityContext.AddAsync(entity);
        var retVal = await _identityContext.SaveChangesAsync();
        if (retVal > 0)
            return new Result<OTP>()
            {
                Data = entity,
                Succeeded = true,
                Message = "Thêm mới dữ liệu thành công!",
            };
        return new Result<OTP>()
        {
            Data = entity,
            Succeeded = false,
            Message = "Thêm mới dữ liệu không thành công",
        };
    }

    public async Task<List<OTP>> GetAllAsync(Expression<Func<OTP, bool>> predicate)
    {
        return await _identityContext.Set<OTP>().Where(predicate).ToListAsync();
    }

    public async Task<Result<OTP>> UpdateAsync(OTP entity)
    {
        try
        {
            _identityContext.Entry(entity).State = EntityState.Modified;
            var retval = await _identityContext.SaveChangesAsync();
            if (retval > 0)
                return new Result<OTP>(entity, "Cập nhật thành công!", true);
            return new Result<OTP>(entity, "Cập nhật không thành công.", false);
        }
        catch (Exception e)
        {
            return new Result<OTP>(entity, $"Cập nhật lỗi: {e.Message}", false);
        }
    }
}
