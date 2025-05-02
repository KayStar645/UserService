using Ardalis.Result;
using Ardalis.SharedKernel;
using FluentValidation;
using Microsoft.Extensions.Localization;
using UserService.Application.Resources.Languages;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Base.Queries;

public abstract record GetQueryDto<TKey, TDto> : IQuery<Result<TDto>>
{
    public required TKey Id { get; init; }
    public string[]? Includes { get; init; }
    public string[]? Fields { get; init; }
}

public class GetQueryValidator<TKey, TRequest, TDto> : AbstractValidator<TRequest> where TRequest : GetQueryDto<TKey, TDto>
{
    public GetQueryValidator(IUnitOfWork<TKey> pUnitOfWork, IStringLocalizer<SharedResource> pValidatorLocalizer)
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(pValidatorLocalizer["NameRequired", "Id"]);
    }
}