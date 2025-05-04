using Ardalis.Result;
using Microsoft.EntityFrameworkCore;

namespace UserService.Application.Features.Base.Queries;

public class PagedData<T>
{
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }

    public bool HasNextPage => Page < TotalPages;
    public bool HasPreviousPage => Page > 1;
}

public class PagedListResult<T> : Result<PagedData<T>>
{
    private PagedListResult() { }

    public static PagedListResult<T> Success(IEnumerable<T> items, int totalCount, int page = 1, int pageSize = 30)
    {
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        var value = new PagedData<T>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = totalPages
        };

        return new PagedListResult<T>
        {
            Value = value,
            Status = ResultStatus.Ok
        };
    }

    public static new PagedListResult<T> Error(string errorMessage)
    {
        return new PagedListResult<T>
        {
            Status = ResultStatus.Error,
            Errors = new List<string> { errorMessage }
        };
    }

    public static PagedListResult<T> Invalid(IEnumerable<string> validationErrors)
    {
        return new PagedListResult<T>
        {
            Status = ResultStatus.Invalid,
            Errors = validationErrors.ToList()
        };
    }
}

public static class QueryableExtensions
{
    public static async Task<PagedListResult<TEntity>> GetPagedDataAsync<TEntity>(this IQueryable<TEntity> pQuery, int? pPage, int? pPageSize)
    {
        int page = pPage ?? 1;
        int pageSize = pPageSize ?? 30;
        int totalCount = await pQuery.CountAsync();
        var items = await pQuery.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return PagedListResult<TEntity>.Success(items, totalCount, page, pageSize);
    }
}

