using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using UserService.Application.Features.Base.Commands;
using UserService.Application.Resources;
using UserService.Domain.Common.Constants;
using UserService.Domain.DTOs;
using UserService.Domain.Entities;
using UserService.Domain.Enums;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Users.Commands;

public record UpdateUserDto : UpdateCommandDto<Guid, UserDto>
{
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

public class UpdateUserValidator : UpdateCommandValidator<Guid, UpdateUserDto, UserDto>
{
    public UpdateUserValidator(IUnitOfWork<Guid> pUnitOfWork, IStringLocalizer<SharedResource> pSharedLocalizer) : base(pUnitOfWork, pSharedLocalizer)
    {
        RuleFor(x => x.Username)
           .NotEmpty().WithMessage(pSharedLocalizer["NameRequired", nameof(UpdateUserDto.Username)])
           .MaximumLength(FieldLengthConstants.CodeMaxLength).WithMessage(pSharedLocalizer["MaximumLength", nameof(UpdateUserDto.Username), FieldLengthConstants.CodeMaxLength])
           .MustAsync(async (dto, username, cancellationToken) =>
           {
               var current = await pUnitOfWork.Set<User>().GetByCondition(x => x.Id == dto.Id).Select(x => new { x.CompanyId, x.BranchId }).FirstOrDefaultAsync();
               if (current == null) return true;

               return !await pUnitOfWork.Set<User>().AnyAsync(x => x.Username == username && x.CompanyId == current.CompanyId && x.BranchId == current.BranchId && x.Id != dto.Id);
           }).WithMessage(pSharedLocalizer["Exists", nameof(UpdateUserDto.Username)]);

        RuleFor(x => x.Email)
           .MaximumLength(FieldLengthConstants.EmailMaxLength)
           .WithMessage(pSharedLocalizer["MaximumLength", nameof(UpdateUserDto.Email), FieldLengthConstants.EmailMaxLength])
           .EmailAddress().When(x => !string.IsNullOrWhiteSpace(x.Email))
           .WithMessage(pSharedLocalizer["InvalidValue", nameof(UpdateUserDto.Email)])
           .MustAsync(async (dto, email, cancellationToken) =>
           {
               if (string.IsNullOrWhiteSpace(email)) return true;

               var current = await pUnitOfWork.Set<User>().GetByCondition(x => x.Id == dto.Id).Select(x => new { x.CompanyId, x.BranchId }).FirstOrDefaultAsync();
               if (current == null) return true;

               return !await pUnitOfWork.Set<User>().AnyAsync(x => x.Email == email && x.CompanyId == current.CompanyId && x.BranchId == current.BranchId && x.Id != dto.Id);
           }).WithMessage(pSharedLocalizer["Exists", nameof(UpdateUserDto.Email)]);

        RuleFor(x => x.PhoneNumber)
           .MaximumLength(FieldLengthConstants.PhoneNumberMaxLength)
           .WithMessage(pSharedLocalizer["MaximumLength", nameof(UpdateUserDto.PhoneNumber), FieldLengthConstants.PhoneNumberMaxLength])
           .Matches(@"^\+?\d{7,15}$").When(x => !string.IsNullOrWhiteSpace(x.PhoneNumber))
           .WithMessage(pSharedLocalizer["InvalidValue", nameof(UpdateUserDto.PhoneNumber)])
           .MustAsync(async (dto, phone, cancellationToken) =>
           {
               if (string.IsNullOrWhiteSpace(phone)) return true;

               var current = await pUnitOfWork.Set<User>().GetByCondition(x => x.Id == dto.Id).Select(x => new { x.CompanyId, x.BranchId }).FirstOrDefaultAsync();
               if (current == null) return true;

               return !await pUnitOfWork.Set<User>().AnyAsync(x => x.PhoneNumber == phone && x.CompanyId == current.CompanyId && x.BranchId == current.BranchId &&x.Id != dto.Id);
           }).WithMessage(pSharedLocalizer["Exists", nameof(UpdateUserDto.PhoneNumber)]);

        RuleFor(x => x.AvatarUrl)
            .MaximumLength(FieldLengthConstants.UrlMaxLength)
            .WithMessage(pSharedLocalizer["MaximumLength", nameof(UpdateUserDto.AvatarUrl), FieldLengthConstants.UrlMaxLength])
            .Matches(@"^(https?|ftp):\/\/[^\s/$.?#].[^\s]*$").When(x => !string.IsNullOrWhiteSpace(x.AvatarUrl))
            .WithMessage(pSharedLocalizer["InvalidValue", nameof(UpdateUserDto.AvatarUrl)]);

        RuleFor(x => x.DateOfBirth)
            .LessThanOrEqualTo(DateTimeOffset.UtcNow)
            .When(x => x.DateOfBirth.HasValue)
            .WithMessage(pSharedLocalizer["InvalidValue", nameof(UpdateUserDto.DateOfBirth)]);

        RuleFor(x => x.Gender)
            .Must(g => g == null || Enum.IsDefined(typeof(EGender), g))
            .WithMessage(pSharedLocalizer["InvalidValue", nameof(UpdateUserDto.Gender)]);

        RuleFor(x => x.Id)
            .MustAsync(async (id, cancellationToken) =>
            {
                return await pUnitOfWork.Set<User>().AnyAsync(x => x.Id == id);
            }).WithMessage(pSharedLocalizer["NotFound", nameof(UpdateUserDto.Id)]);
    }
}
