using Microsoft.EntityFrameworkCore;
using Share.Core.Pagination;

namespace Shared.Core.Pagination
{
    public static class QueryableExtensions
    {
        public static async Task<PaginatedResultExt<T>> ToPaginatedListAsync<T>(this IQueryable<T> source, int? pageNumber, int? pageSize, IUriService uriService, string route, CancellationToken? cancellationToken = null) where T : class
        {
            try
            {
                pageNumber = pageNumber == null || pageNumber == 0 ? 1 : pageNumber;
                pageSize = pageSize == null || pageSize == 0 ? 10 : pageSize;
                int count = await source.CountAsync();
                List<T> items;
                if (cancellationToken != null)
                    items = await source.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value).ToListAsync(cancellationToken.Value);
                else
                    items = await source.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value).ToListAsync();
                //items = await source.Skip(1).Take(2).ToListAsync();
                //items = await source.ToListAsync();
                return PaginatedResultExt<T>.Create(items, count, pageNumber.Value, pageSize.Value, uriService, route);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
