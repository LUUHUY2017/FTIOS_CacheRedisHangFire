﻿using Microsoft.EntityFrameworkCore;
using Shared.Core.Commons;
using Shared.Core.Entities;
using Shared.Core.Repositories;
using System.Linq.Expressions;

namespace Server.Infrastructure.Datas.MasterData;

public class RepositoryBaseMasterData<T> : IAsyncRepository<T> where T : EntityBase
{
    protected readonly MasterDataDbContext _dbContext;

    public RepositoryBaseMasterData(MasterDataDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await _dbContext.Set<T>().ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbContext.Set<T>().Where(predicate).ToListAsync();
    }

    public async Task<T> GetByIdAsync(string id)
    {
        return await _dbContext.Set<T>().FindAsync(id)!;
    }

    public async Task<T> AddAsync(T entity)
    {
        _dbContext.Set<T>().Add(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<Result<int>> DeleteAsync(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
        var retVal = await _dbContext.SaveChangesAsync();
        return new Result<int>(retVal);
    }
}
