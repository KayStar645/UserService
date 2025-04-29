using Ardalis.Result;
using Ardalis.SharedKernel;
using FluentValidation;
using Microsoft.Extensions.Localization;
using UserService.Application.Features.Base.Commands.Create;
using UserService.Application.Features.Languages;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Base.Commands.Update;

public abstract record UpdateCommand<TKey, TDto> : ICommand<Result<TDto>>
{
    public required TKey Id { get; set; }
}


public class UpdateCommandValidator<TKey, TRequest, TDto> : AbstractValidator<TRequest> where TRequest : CreateCommand<TDto>
{
    public UpdateCommandValidator(IUnitOfWork<TKey> pUnitOfWork, IStringLocalizer<LValidator> pValidatorLocalizer)
    {
        RuleFor(x => x)
            .NotNull().WithMessage(pValidatorLocalizer["ValidationError"]);
    }
}