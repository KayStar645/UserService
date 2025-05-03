using Microsoft.Extensions.Localization;
using UserService.Application.DTOs;
using UserService.Application.Features.Base.Commands;
using UserService.Application.Resources;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Permissions.Commands;

public record CreatePermissionDto : CreateCommandDto<PermissionDto>
{
    public string? CompanyId { get; set; }
    public string? BranchId { get; set; }
    public string? Code { get; set; }
    public string? Name { get; set; }
}

public class CreatePermissionValidator : CreateCommandValidator<Guid, CreatePermissionDto, PermissionDto>
{
    public CreatePermissionValidator(IUnitOfWork<Guid> pUnitOfWork, IStringLocalizer<SharedResource> pSharedResourceLocalizer) : base(pUnitOfWork, pSharedResourceLocalizer)
    {
        
    }
}