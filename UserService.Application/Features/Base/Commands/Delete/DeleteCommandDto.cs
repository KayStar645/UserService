using Ardalis.Result;
using Ardalis.SharedKernel;
using FluentValidation;
using Microsoft.Extensions.Localization;
using UserService.Application.Resources.Languages;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Base.Commands;

public abstract record DeleteCommandDto<TKey> : ICommand<Result>
{
    public required TKey Id { get; set; }
}

public class DeleteCommandValidator<TKey, TRequest> : AbstractValidator<TRequest> where TRequest : DeleteCommandDto<TKey>
{
    public DeleteCommandValidator(IUnitOfWork<TKey> pUnitOfWork, IStringLocalizer<LValidator> pValidatorLocalizer)
    {
        RuleFor(x => x)
            .NotNull().WithMessage(pValidatorLocalizer["ValidationError"]);
    }
}