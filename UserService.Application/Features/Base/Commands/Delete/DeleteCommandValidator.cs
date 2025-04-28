using FluentValidation;
using Microsoft.Extensions.Localization;
using UserService.Application.Features.Languages;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Base.Commands.Delete;

public class DeleteCommandValidator<TKey, TRequest> : AbstractValidator<TRequest> where TRequest : DeleteCommand<TKey>
{
    public DeleteCommandValidator(IUnitOfWork<TKey> pUnitOfWork, IStringLocalizer<LValidator> pValidatorLocalizer)
    {
        RuleFor(x => x)
            .NotNull().WithMessage(pValidatorLocalizer["ValidationError"]);
    }
}

