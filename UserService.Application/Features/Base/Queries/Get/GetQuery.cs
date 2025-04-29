using Ardalis.Result;
using Ardalis.SharedKernel;
using FluentValidation;
using Microsoft.Extensions.Localization;
using UserService.Application.Features.Languages;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Base.Queries.Get;

public abstract record GetQuery<TKey, TDto> : IQuery<Result<TDto>>
{
    public required TKey Id { get; set; }
    public List<string>? Includes;
    public List<string>? Fields;
}

public class GetQueryValidator<TKey, TRequest, TDto> : AbstractValidator<TRequest> where TRequest : GetQuery<TKey, TDto>
{
    public GetQueryValidator(IUnitOfWork<TKey> pUnitOfWork, IStringLocalizer<LValidator> pValidatorLocalizer)
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(pValidatorLocalizer["NameRequired", "Id"]);
    }
}