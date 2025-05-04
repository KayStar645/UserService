using FluentValidation;
using Microsoft.Extensions.Localization;
using UserService.Domain.DTOs;
using UserService.Application.Features.Base.Queries;
using UserService.Application.Resources;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Roles.Queries;

public record GetRoleDto : GetQueryDto<Guid, RoleDto>
{
}

public class GetRoleValidator : AbstractValidator<GetRoleDto>
{
    public GetRoleValidator(IUnitOfWork<Guid> pUnitOfWork, IStringLocalizer<SharedResource> pSharedLocalizer)
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(pSharedLocalizer["NameRequired", "Id"]);
    }
}
