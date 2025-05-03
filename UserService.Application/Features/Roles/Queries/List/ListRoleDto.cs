using Microsoft.Extensions.Localization;
using UserService.Application.DTOs;
using UserService.Application.Features.Base.Queries;
using UserService.Application.Resources;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Roles.Queries;

public record ListRoleDto : ListQueryDto<RoleDto>
{
}

public class ListRoleValidator : ListQueryValidator<Guid, ListRoleDto, RoleDto>
{
    public ListRoleValidator(IUnitOfWork<Guid> pUnitOfWork, IStringLocalizer<SharedResource> pSharedLocalizer)
        : base(pUnitOfWork, pSharedLocalizer)
    { }
}
