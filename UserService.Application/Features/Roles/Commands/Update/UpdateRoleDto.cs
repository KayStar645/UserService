using FluentValidation;
using Microsoft.Extensions.Localization;
using UserService.Application.DTOs;
using UserService.Application.Features.Base.Commands;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Roles.Commands;

public record UpdateRoleDto : UpdateCommandDto<Guid, RoleDto>
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public required IEnumerable<UpdateRolePermissionDto> RolePermissions { get; set; }
}


public class UpdateRoleValidator : AbstractValidator<UpdateRoleDto>
{
    public UpdateRoleValidator(IUnitOfWork<Guid> pUnitOfWork, IStringLocalizer pValidatorLocalizer)
    {
        RuleFor(x => x)
            .NotNull().WithMessage(pValidatorLocalizer["ValidationError"]);
    }
}
