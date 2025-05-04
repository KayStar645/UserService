using Microsoft.Extensions.Localization;
using UserService.Domain.DTOs;
using UserService.Application.Features.Base.Commands;
using UserService.Application.Resources;
using UserService.Domain.Enums;
using UserService.Infrastructure.Repositories.Interfaces;
using UserService.Application.Features.Permissions.Commands;
using UserService.Domain.Common.Constants;
using FluentValidation;
using UserService.Domain.Entities;

namespace UserService.Application.Features.Users.Commands;

public record CreateUserDto : CreateCommandDto<UserDto>
{
    public string? CompanyId { get; set; }
    public string? BranchId { get; set; }
    public required string Username { get; set; }
    public string? Email { get; set; }
    public bool IsEmailConfirmed { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IsPhoneNumberConfirmed { get; set; }
    public string? FullName { get; set; }
    public string? AvatarUrl { get; set; }
    public DateTimeOffset? DateOfBirth { get; set; }
    public EGender? Gender { get; set; }
}


public class CreateUserValidator : CreateCommandValidator<Guid, CreateUserDto, UserDto>
{
    public CreateUserValidator(IUnitOfWork<Guid> pUnitOfWork, IStringLocalizer<SharedResource> pSharedLocalizer) : base(pUnitOfWork, pSharedLocalizer)
    {
        RuleFor(x => x.Username)
           .NotEmpty().WithMessage(pSharedLocalizer["NameRequired", nameof(CreatePermissionDto.Code)])
           .MaximumLength(FieldLengthConstants.CodeMaxLength).WithMessage(pSharedLocalizer["MaximumLength", nameof(CreatePermissionDto.Code), FieldLengthConstants.CodeMaxLength])
           .MustAsync(async (dto, userName, cancellationToken) =>
           {
               return !await pUnitOfWork.Set<User>()
                   .AnyAsync(x => x.Username == userName && x.CompanyId == dto.CompanyId && x.BranchId == dto.BranchId);
           })
           .WithMessage(pSharedLocalizer["Exists", nameof(CreatePermissionDto.Code)]);

        RuleFor(x => x.Email)
           .MaximumLength(FieldLengthConstants.EmailMaxLength).WithMessage(pSharedLocalizer["MaximumLength", nameof(CreateUserDto.Email), FieldLengthConstants.EmailMaxLength])
           .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email)).WithMessage(pSharedLocalizer["InvalidValue", nameof(CreateUserDto.Email)])
           .MustAsync(async (dto, email, cancellationToken) =>
           {
               if (string.IsNullOrWhiteSpace(email)) return true;
               return !await pUnitOfWork.Set<User>().AnyAsync(x => x.Email == email && x.CompanyId == dto.CompanyId && x.BranchId == dto.BranchId);
           })
           .WithMessage(pSharedLocalizer["Exists", nameof(CreateUserDto.Email)]);

        RuleFor(x => x.PhoneNumber)
           .MaximumLength(FieldLengthConstants.PhoneNumberMaxLength).WithMessage(pSharedLocalizer["MaximumLength", nameof(CreateUserDto.PhoneNumber), FieldLengthConstants.PhoneNumberMaxLength])
           .Matches(@"^\+?\d{7,15}$").When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber)).WithMessage(pSharedLocalizer["InvalidValue", nameof(CreateUserDto.PhoneNumber)])
           .MustAsync(async (dto, phone, cancellationToken) =>
           {
               if (string.IsNullOrWhiteSpace(phone)) return true;
               return !await pUnitOfWork.Set<User>().AnyAsync(x => x.PhoneNumber == phone && x.CompanyId == dto.CompanyId && x.BranchId == dto.BranchId);
           })
           .WithMessage(pSharedLocalizer["Exists", nameof(CreateUserDto.PhoneNumber)]);

        RuleFor(x => x.AvatarUrl)
            .MaximumLength(FieldLengthConstants.UrlMaxLength).WithMessage(pSharedLocalizer["MaximumLength", nameof(CreateUserDto.AvatarUrl), FieldLengthConstants.UrlMaxLength])
            .Matches(@"^(https?|ftp):\/\/[^\s/$.?#].[^\s]*$")
            .When(x => !string.IsNullOrWhiteSpace(x.AvatarUrl)).WithMessage(pSharedLocalizer["InvalidValue", nameof(CreateUserDto.AvatarUrl)]);

        RuleFor(x => x.DateOfBirth)
            .LessThanOrEqualTo(DateTimeOffset.UtcNow).When(x => x.DateOfBirth.HasValue)
            .WithMessage(pSharedLocalizer["InvalidValue", nameof(CreateUserDto.DateOfBirth)]);

        RuleFor(x => x.Gender)
            .Must(gender => gender == null || Enum.IsDefined(typeof(EGender), gender))
            .WithMessage(pSharedLocalizer["InvalidValue", nameof(CreateUserDto.Gender)]);
    }
}