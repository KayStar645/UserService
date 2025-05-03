using Ardalis.Result;
using Ardalis.SharedKernel;
using FluentValidation;
using Microsoft.Extensions.Localization;
using UserService.Application.Resources;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Base.Commands;

public abstract record UpdateCommandDto<TKey, TDto> : ICommand<Result<TDto>>
{
    public required TKey Id { get; set; }
}


public class UpdateCommandValidator<TKey, TRequest, TDto> : AbstractValidator<TRequest> where TRequest : UpdateCommandDto<TKey, TDto>
{
    public UpdateCommandValidator(IUnitOfWork<TKey> pUnitOfWork, IStringLocalizer<SharedResource> pSharedResourceLocalizer)
    {
        RuleFor(x => x)
            .NotNull().WithMessage(pSharedResourceLocalizer["ValidationError"]);
    }
}