using Microsoft.Extensions.Localization;
using UserService.Application.DTOs;
using UserService.Application.Features.Base.Queries;
using UserService.Application.Resources.Languages;
using UserService.Domain.Common.Entity.Interfaces;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Roles.Queries;

public record ListRoleDto : ListQueryDto<RoleDto>, IOrganizationScope
{
    public string? CompanyId { get; set; }
    public string? BranchId { get; set; }
}

public class ListRoleValidator : ListQueryValidator<Guid, ListRoleDto, RoleDto>
{
    public ListRoleValidator(IUnitOfWork<Guid> pUnitOfWork, IStringLocalizer<LValidator> pValidatorLocalizer)
        : base(pUnitOfWork, pValidatorLocalizer)
    { }
}
