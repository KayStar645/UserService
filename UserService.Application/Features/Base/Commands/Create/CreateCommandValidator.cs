using FluentValidation;
using Microsoft.Extensions.Localization;
using UserService.Application.Features.Languages;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Base.Commands.Create;

public class CreateCommandValidator<TKey, TRequest, TDto> : AbstractValidator<TRequest> where TRequest : CreateCommand<TDto>
{
    public CreateCommandValidator(IUnitOfWork<TKey> pUnitOfWork, IStringLocalizer<LValidator> pValidatorLocalizer)
    {
        RuleFor(x => x)
            .NotNull().WithMessage(pValidatorLocalizer["ValidationError"]);
    }
}



