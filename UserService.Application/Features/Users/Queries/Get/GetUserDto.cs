using FluentValidation;
using Microsoft.Extensions.Localization;
using UserService.Domain.DTOs;
using UserService.Application.Features.Base.Queries;
using UserService.Application.Resources;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Users.Queries;

public record GetUserDto : GetQueryDto<Guid, UserDto>
{
}

public class GetUserValidator : AbstractValidator<GetUserDto>
{
    public GetUserValidator(IUnitOfWork<Guid> pUnitOfWork, IStringLocalizer<SharedResource> pSharedResourceLocalizer)
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(pSharedResourceLocalizer["NameRequired", "Id"]);
    }
}
