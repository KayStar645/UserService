using Microsoft.Extensions.Localization;
using UserService.Application.DTOs;
using UserService.Application.Features.Base.Queries;
using UserService.Application.Resources.Languages;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Permissions.Queries;

public record ListPermissionDto : ListQueryDto<PermissionDto>
{
}

public class ListPermissionValidator : ListQueryValidator<Guid, ListPermissionDto, PermissionDto>
{
    public ListPermissionValidator(IUnitOfWork<Guid> pUnitOfWork, IStringLocalizer<LValidator> pValidatorLocalizer)
        :base(pUnitOfWork, pValidatorLocalizer)
    { }
}
