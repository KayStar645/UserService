using Ardalis.Result;
using Microsoft.EntityFrameworkCore;

namespace UserService.Application.Features.Base.Queries;

public class PagedListResult<TEntity> : Result
{
    public IEnumerable<TEntity> Items { get; }
    public int TotalCount { get; }
    public int Page { get; }
    public int TotalPages { get; }
    public int PageSize { get; }
    public bool HasNextPage => Page < TotalPages;
    public bool HasPreviousPage => Page > 1;

    public PagedListResult(IEnumerable<TEntity> items, int totalCount, int? pPage = 1, int? pPageSize = 30)
    {
        int page = pPage ?? 1;
        int pageSize = pPageSize ?? 30;

        Items = items;
        TotalCount = totalCount;
        Page = page;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling((double)TotalCount / PageSize);
    }

    public static PagedListResult<TEntity> Success(IEnumerable<TEntity> items, int totalCount, int page = 1, int pageSize = 30)
    {
        return new PagedListResult<TEntity>(items, totalCount, page, pageSize)
        {
            Status = ResultStatus.Ok
        };
    }

    public static new PagedListResult<TEntity> Error(string errorMessage)
    {
        return new PagedListResult<TEntity>(Enumerable.Empty<TEntity>(), 0)
        {
            Status = ResultStatus.Error,
            Errors = new List<string> { errorMessage }
        };
    }

    public static PagedListResult<TEntity> Invalid(IEnumerable<string> validationErrors)
    {
        return new PagedListResult<TEntity>(Enumerable.Empty<TEntity>(), 0)
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

