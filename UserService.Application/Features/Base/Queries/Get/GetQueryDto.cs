using Ardalis.Result;
using Ardalis.SharedKernel;
using FluentValidation;
using Microsoft.Extensions.Localization;
using UserService.Application.Resources.Languages;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Base.Queries;

public abstract record GetQueryDto<TKey, TDto> : IQuery<Result<TDto>>
{
    public required TKey Id { get; set; }
    public List<string>? Includes;
    public List<string>? Fields;
}

public class GetQueryValidator<TKey, TRequest, TDto> : AbstractValidator<TRequest> where TRequest : GetQueryDto<TKey, TDto>
{
    public GetQueryValidator(IUnitOfWork<TKey> pUnitOfWork, IStringLocalizer<LValidator> pValidatorLocalizer)
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(pValidatorLocalizer["NameRequired", "Id"]);
    }
}