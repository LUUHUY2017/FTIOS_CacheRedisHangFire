namespace Share.Core.Pagination;

public interface IUriService
{
    public Uri GetPageUri(int pageNumber, int pageSize, string route);
}
