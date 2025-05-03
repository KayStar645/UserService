using Microsoft.Extensions.Localization;
using UserService.Domain.DTOs;
using UserService.Application.Features.Base.Queries;
using UserService.Application.Resources;
using UserService.Domain.Common.Entity.Interfaces;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Permissions.Queries;

public record ListPermissionDto : ListQueryDto<PermissionDto>, IOrganizationScope
{
    public string? CompanyId { get; set; }
    public string? BranchId { get; set; }
}

public class ListPermissionValidator : ListQueryValidator<Guid, ListPermissionDto, PermissionDto>
{
    public ListPermissionValidator(IUnitOfWork<Guid> pUnitOfWork, IStringLocalizer<SharedResource> pSharedResourceLocalizer)
        :base(pUnitOfWork, pSharedResourceLocalizer)
    { }
}
