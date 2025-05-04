using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using UserService.Application.Features.Base.Commands;
using UserService.Application.Resources;
using UserService.Domain.Common.Constants;
using UserService.Domain.DTOs;
using UserService.Domain.Entities;
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
        RuleFor(x => x.Code)
           .NotEmpty().WithMessage(pSharedLocalizer["NameRequired", nameof(CreateRoleDto.Code)])
           .MaximumLength(FieldLengthConstants.CodeMaxLength).WithMessage(pSharedLocalizer["MaximumLength", nameof(CreateRoleDto.Code), FieldLengthConstants.CodeMaxLength])
           .MustAsync(async (dto, code, cancellationToken) =>
           {
               return !await pUnitOfWork.Set<Role>()
                   .AnyAsync(x => x.Code == code && x.CompanyId == dto.CompanyId && x.BranchId == dto.BranchId);
           })
           .WithMessage(pSharedLocalizer["Exists", nameof(CreateRoleDto.Code)]);

        RuleFor(x => x.Name)
            .MaximumLength(FieldLengthConstants.NameMaxLength).WithMessage(pSharedLocalizer["MaximumLength", nameof(CreateRoleDto.Name), FieldLengthConstants.NameMaxLength]);

        RuleFor(x => x.RolePermissions)
            .NotEmpty().WithMessage(pSharedLocalizer["Required", nameof(CreateRoleDto.RolePermissions)])
            .MustAsync(async (dto, rolePermissions, cancellationToken) =>
            {
                if (rolePermissions == null || !rolePermissions.Any())
                    return false;

                var permissionIds = rolePermissions.Select(rp => rp.PermissionId).ToList();

                var permissions = await pUnitOfWork.Set<Permission>()
                            .GetByCondition(p => permissionIds.Contains(p.Id))
                            .Select(p => new { p.Id, p.CompanyId, p.BranchId })
                            .ToListAsync(cancellationToken);

                // Check đủ số lượng và cùng CompanyId, BranchId
                if (permissions.Count != permissionIds.Count)
                    return false;

                return permissions.All(p => p.CompanyId == dto.CompanyId && p.BranchId == dto.BranchId);
            })
            .WithMessage(pSharedLocalizer["InvalidValue", nameof(CreateRoleDto.RolePermissions)]);

    }
}