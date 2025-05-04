using Ardalis.Result;
using Ardalis.SharedKernel;
using FluentValidation;
using Microsoft.Extensions.Localization;
using UserService.Application.Resources;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Base.Commands;

public abstract record CreateCommandDto<TDto> : ICommand<Result<TDto>>;

public class CreateCommandValidator<TKey, TRequest, TDto> : AbstractValidator<TRequest> where TRequest : CreateCommandDto<TDto>
{
    public CreateCommandValidator(IUnitOfWork<TKey> pUnitOfWork, IStringLocalizer<SharedResource> pSharedLocalizer)
    {
        RuleFor(x => x)
            .NotNull().WithMessage(pSharedLocalizer["ValidationError"]);
    }
}