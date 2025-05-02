using Ardalis.Result;
using Ardalis.SharedKernel;
using FluentValidation;
using Microsoft.Extensions.Localization;
using UserService.Application.Resources.Languages;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Base.Queries;

public abstract record ListQueryDto<TDto> : IQuery<Result<PagedListResult<TDto>>>
{
    public string? Search { get; init; }
    public string? Filters { get; init; }
    public string? Sorts { get; init; }
    public int? Page { get; init; } = 1;
    public int? PageSize { get; init; } = 30;
    public string[]? Includes { get; init; }
    public string[]? Fields { get; init; }
}

public class ListQueryValidator<TKey, TRequest, TDto> : AbstractValidator<TRequest> where TRequest : ListQueryDto<TDto>
{
    public ListQueryValidator(IUnitOfWork<TKey> pUnitOfWork, IStringLocalizer<LValidator> pValidatorLocalizer)
    { }
}