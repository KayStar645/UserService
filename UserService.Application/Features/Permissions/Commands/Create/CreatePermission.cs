using Microsoft.Extensions.Localization;
using UserService.Application.DTOs;
using UserService.Application.Features.Base.Commands.Create;
using UserService.Application.Features.Languages;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Permissions.Commands.Create;

public record CreatePermission : CreateCommand<PermissionDto>
{
    public string? Code { get; set; }
    public string? Name { get; set; }
}

public class CreatePermissionValidator : CreateCommandValidator<Guid, CreatePermission, PermissionDto>
{
    public CreatePermissionValidator(IUnitOfWork<Guid> pUnitOfWork, IStringLocalizer<LValidator> pValidatorLocalizer) : base(pUnitOfWork, pValidatorLocalizer)
    {
        
    }
}