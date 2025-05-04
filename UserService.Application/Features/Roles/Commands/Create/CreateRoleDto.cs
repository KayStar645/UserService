using Microsoft.Extensions.Localization;
using UserService.Domain.DTOs;
using UserService.Application.Features.Base.Commands;
using UserService.Application.Resources;
using UserService.Infrastructure.Repositories.Interfaces;
using FluentValidation;
using UserService.Application.Features.Permissions.Commands;
using UserService.Domain.Common.Constants;
using UserService.Domain.Entities;

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
        RuleFor(x => x.Code)
           .NotEmpty().WithMessage(pSharedLocalizer["NameRequired", nameof(CreatePermissionDto.Code)])
           .MaximumLength(FieldLengthConstants.CodeMaxLength).WithMessage(pSharedLocalizer["MaximumLength", nameof(CreatePermissionDto.Code), FieldLengthConstants.CodeMaxLength])
           .MustAsync(async (dto, code, cancellationToken) =>
           {
               return !await pUnitOfWork.Set<Role>()
                   .AnyAsync(x => x.Code == code && x.CompanyId == dto.CompanyId && x.BranchId == dto.BranchId);
           })
           .WithMessage(pSharedLocalizer["Exists", nameof(CreatePermissionDto.Code)]);
    }
}