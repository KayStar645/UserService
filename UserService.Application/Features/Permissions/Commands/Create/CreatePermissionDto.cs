using Microsoft.Extensions.Localization;
using UserService.Domain.DTOs;
using UserService.Application.Features.Base.Commands;
using UserService.Application.Resources;
using UserService.Infrastructure.Repositories.Interfaces;
using FluentValidation;
using UserService.Domain.Entities;
using UserService.Domain.Common.Constants;

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
    public CreatePermissionValidator(IUnitOfWork<Guid> pUnitOfWork, IStringLocalizer<SharedResource> pSharedLocalizer) : base(pUnitOfWork, pSharedLocalizer)
    {
        RuleFor(x => x.Code)
           .NotEmpty().WithMessage(pSharedLocalizer["NameRequired", nameof(CreatePermissionDto.Code)])
           .MaximumLength(FieldLengthConstants.CodeMaxLength).WithMessage(pSharedLocalizer["MaximumLength", nameof(CreatePermissionDto.Code), FieldLengthConstants.CodeMaxLength])
           .MustAsync(async (dto, code, cancellationToken) =>
           {
               return !await pUnitOfWork.Set<Permission>()
                   .AnyAsync(x => x.Code == code && x.CompanyId == dto.CompanyId && x.BranchId == dto.BranchId);
           })
           .WithMessage(pSharedLocalizer["Exists", nameof(CreatePermissionDto.Code)]);

    }
}