using Ardalis.Result;
using Ardalis.SharedKernel;
using FluentValidation;
using Microsoft.Extensions.Localization;
using UserService.Application.Features.Languages;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Base.Queries.List;

public abstract record ListQuery<TDto> : IQuery<Result<PagedListResult<TDto>>>
{
    public string? Search;
    public string? Filters;
    public string? Sorts;
    public int Page = 1;
    public int PageSize = 30;
    public List<string>? Includes;
    public List<string>? Fields;

}

public class ListQueryValidator<TKey, TRequest, TDto> : AbstractValidator<TRequest> where TRequest : ListQuery<TDto>
{
    public ListQueryValidator(IUnitOfWork<TKey> pUnitOfWork, IStringLocalizer<LValidator> pValidatorLocalizer)
    { }
}