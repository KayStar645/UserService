using Microsoft.Extensions.Localization;
using UserService.Application.DTOs;
using UserService.Application.Features.Base.Queries.List;
using UserService.Application.Features.Languages;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Permissions.Queries.List;

public record ListPermission : ListQuery<PermissionDto>
{
}

public class ListPermissionValidator : ListQueryValidator<Guid, ListPermission, PermissionDto>
{
    public ListPermissionValidator(IUnitOfWork<Guid> pUnitOfWork, IStringLocalizer<LValidator> pValidatorLocalizer)
        :base(pUnitOfWork, pValidatorLocalizer)
    { }
}
