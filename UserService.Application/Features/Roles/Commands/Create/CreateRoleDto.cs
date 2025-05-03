using Microsoft.Extensions.Localization;
using UserService.Application.DTOs;
using UserService.Application.Features.Base.Commands;
using UserService.Application.Resources.Languages;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Roles.Commands;

public record CreateRoleDto : CreateCommandDto<RoleDto>
{
    public string? CompanyId { get; set; }
    public string? BranchId { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
    public required IEnumerable<CreateRolePermissionDto> RolePermissions { get; set; }
}

public class CreateRoleValidator : CreateCommandValidator<Guid, CreateRoleDto, RoleDto>
{
    public CreateRoleValidator(IUnitOfWork<Guid> pUnitOfWork, IStringLocalizer<SharedResource> pSharedLocalizer) : base(pUnitOfWork, pSharedLocalizer)
    {

    }
}