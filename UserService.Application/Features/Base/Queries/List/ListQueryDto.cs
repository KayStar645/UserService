using Ardalis.SharedKernel;
using FluentValidation;
using Microsoft.Extensions.Localization;
using UserService.Application.Resources;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Base.Queries;

public abstract record ListQueryDto<TDto> : IQuery<PagedListResult<TDto>>
{
    public string? Search { get; set; }
    public string? Filters { get; set; }
    public string? Sorts { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string[]? Includes { get; set; }
    public string[]? Fields { get; set; }
}

public class ListQueryValidator<TKey, TRequest, TDto> : AbstractValidator<TRequest> where TRequest : ListQueryDto<TDto>
{
    public ListQueryValidator(IUnitOfWork<TKey> pUnitOfWork, IStringLocalizer<SharedResource> pSharedLocalizer)
    { }
}