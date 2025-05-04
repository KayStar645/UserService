using FluentValidation;
using Microsoft.Extensions.Localization;
using UserService.Domain.DTOs;
using UserService.Application.Features.Base.Commands;
using UserService.Infrastructure.Repositories.Interfaces;
using UserService.Application.Features.Permissions.Commands;
using UserService.Domain.Common.Constants;
using UserService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace UserService.Application.Features.Roles.Commands;

public record UpdateRoleDto : UpdateCommandDto<Guid, RoleDto>
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public IEnumerable<UpdateRolePermissionDto> RolePermissions { get; set; }
}

public class UpdateRoleValidator : AbstractValidator<UpdateRoleDto>
{
    public UpdateRoleValidator(IUnitOfWork<Guid> pUnitOfWork, IStringLocalizer pSharedLocalizer)
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage(pSharedLocalizer["NameRequired", nameof(UpdateRoleDto.Code)])
            .MaximumLength(FieldLengthConstants.CodeMaxLength)
                .WithMessage(dto => string.Format(pSharedLocalizer["MaximumLength"], nameof(UpdateRoleDto.Code), FieldLengthConstants.CodeMaxLength))
            .MustAsync(async (dto, code, cancellationToken) =>
            {
                var roleRepo = pUnitOfWork.Set<Role>();

                var current = await roleRepo.GetByCondition(x => x.Id == dto.Id)
                    .Select(x => new { x.CompanyId, x.BranchId })
                    .FirstOrDefaultAsync();

                if (current == null) return false;

                return !await roleRepo.AnyAsync(x => x.Code == code && x.CompanyId == current.CompanyId && x.BranchId == current.BranchId && x.Id != dto.Id);
            })
            .WithMessage(dto => string.Format(pSharedLocalizer["Exists"], nameof(UpdateRoleDto.Code)));
    }
}
