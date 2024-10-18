using Microsoft.EntityFrameworkCore;
using Shared.Core.Commons;

namespace Share.Core.Pagination;

public class PaginatedResultExt<T> : Result<T>
{
    public PaginatedResultExt(List<T> data)
    {
        Data = data;
    }
    public static PaginatedResultExt<T> Create(List<T> data, int count, int pageNumber, int pageSize, IUriService uriService, string route)
    {
        return new PaginatedResultExt<T>(uriService, route, true, data, null, count, pageNumber, pageSize);
    }

    public PaginatedResultExt(IUriService uriService, string route, bool succeeded, List<T> data = default, string message = "", int count = 0, int pageNumber = 1, int pageSize = 10)
    {
        //Data = data;
        Data = data.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
        CurrentPage = pageNumber;
        Succeeded = succeeded;
        Message = message;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalCount = count;

        FirstPage = uriService.GetPageUri(1, pageSize, route);
        LastPage = uriService.GetPageUri(TotalPages, pageSize, route);

        NextPage = HasNextPage ? uriService.GetPageUri(CurrentPage + 1, pageSize, route) : null;
        PreviousPage = HasPreviousPage ? uriService.GetPageUri(CurrentPage - 1, pageSize, route) : null;
    }

    public Uri FirstPage { get; set; }
    public Uri LastPage { get; set; }
    public Uri NextPage { get; set; }
    public Uri PreviousPage { get; set; }


    public new List<T> Data { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public int PageSize { get; set; }

    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;
}


public class PaginatedResult<T> : Result<T>
{
    public PaginatedResult(List<T> data)
    {
        Data = data;
    }
    public static PaginatedResult<T> Create(List<T> data, int count, int pageNumber, int pageSize, IUriService uriService, string route)
    {
        return new PaginatedResult<T>(uriService, route, true, data, null, count, pageNumber, pageSize);
    }

    public PaginatedResult(IUriService uriService, string route, bool succeeded, List<T> data = default, List<string> messages = null, int count = 0, int pageNumber = 1, int pageSize = 10)
    {
        Data = data;
        CurrentPage = pageNumber;
        Succeeded = succeeded;
        Messages = messages;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalCount = count;

        FirstPage = uriService.GetPageUri(1, pageSize, route);
        LastPage = uriService.GetPageUri(TotalPages, pageSize, route);

        NextPage = HasNextPage ? uriService.GetPageUri(CurrentPage + 1, pageSize, route) : null;
        PreviousPage = HasPreviousPage ? uriService.GetPageUri(CurrentPage - 1, pageSize, route) : null;
    }

    public Uri FirstPage { get; set; }
    public Uri LastPage { get; set; }
    public Uri NextPage { get; set; }
    public Uri PreviousPage { get; set; }


    public new List<T> Data { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public int PageSize { get; set; }

    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;
}

public static class QueryableExtensions
{
    public static async Task<PaginatedResult<T>> ToPaginatedListAsync<T>(this IQueryable<T> source, int pageNumber, int pageSize, IUriService uriService, string route, CancellationToken cancellationToken) where T : class
    {
        pageNumber = pageNumber == 0 ? 1 : pageNumber;
        pageSize = pageSize == 0 ? 10 : pageSize;
        int count = await source.CountAsync();
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        List<T> items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);




        return PaginatedResult<T>.Create(items, count, pageNumber, pageSize, uriService, route);
    }


    public static async Task<PaginatedResult<T>> ToPaginatedListAsync1<T>(this IQueryable<T> source, int pageNumber, int pageSize, IUriService uriService, string route) where T : class
    {
        pageNumber = pageNumber == 0 ? 1 : pageNumber;
        pageSize = pageSize == 0 ? 10 : pageSize;
        int count = await source.CountAsync();
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        List<T> items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();


        return PaginatedResult<T>.Create(items, count, pageNumber, pageSize, uriService, route);
    }
    public static PaginatedResult<T> ToPaginatedList<T>(this IEnumerable<T> source, int pageNumber, int pageSize, IUriService uriService, string route) where T : class
    {
        pageNumber = pageNumber == 0 ? 1 : pageNumber;
        pageSize = pageSize == 0 ? 10 : pageSize;
        int count = source.Count();
        pageNumber = pageNumber <= 0 ? 1 : pageNumber;
        List<T> items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        return PaginatedResult<T>.Create(items, count, pageNumber, pageSize, uriService, route);
    }
}