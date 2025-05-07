using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using UserService.Application.Features.Base.Commands;
using UserService.Application.Resources;
using UserService.Domain.Common.Constants;
using UserService.Domain.DTOs;
using UserService.Domain.Entities;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Application.Features.Users.Commands;

public record ChangePasswordDto : UpdateCommandDto<Guid, UserDto>
{
    public string? OldPassword { get; set; }
    public string? NewPassword { get; set; }
    public string? ConfirmPassword { get; set; }
}

public class ChangePasswordValidator : UpdateCommandValidator<Guid, ChangePasswordDto, UserDto>
{
    public ChangePasswordValidator(IUnitOfWork<Guid> pUnitOfWork, IStringLocalizer<SharedResource> pSharedLocalizer) : base(pUnitOfWork, pSharedLocalizer)
    {
        RuleFor(x => x.OldPassword)
            .NotEmpty().WithMessage(pSharedLocalizer["Required", nameof(ChangePasswordDto.OldPassword)])
            .MinimumLength(FieldLengthConstants.User.PasswordMinLength).WithMessage(pSharedLocalizer["MinimumLength", nameof(ChangePasswordDto.OldPassword), FieldLengthConstants.User.PasswordMinLength])
            .MaximumLength(FieldLengthConstants.User.PasswordMaxLength).WithMessage(pSharedLocalizer["MaximumLength", nameof(ChangePasswordDto.OldPassword), FieldLengthConstants.User.PasswordMaxLength]);

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage(pSharedLocalizer["Required", nameof(ChangePasswordDto.NewPassword)])
            .MinimumLength(FieldLengthConstants.User.PasswordMinLength).WithMessage(pSharedLocalizer["MinimumLength", nameof(ChangePasswordDto.NewPassword), FieldLengthConstants.User.PasswordMinLength])
            .MaximumLength(FieldLengthConstants.User.PasswordMaxLength).WithMessage(pSharedLocalizer["MaximumLength", nameof(ChangePasswordDto.NewPassword), FieldLengthConstants.User.PasswordMaxLength])
            .Must((dto, newPassword) => newPassword != dto.OldPassword)
            .WithMessage(pSharedLocalizer["MustNotEqual", nameof(ChangePasswordDto.NewPassword), nameof(ChangePasswordDto.OldPassword)]);

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage(pSharedLocalizer["Required", nameof(ChangePasswordDto.ConfirmPassword)])
            .Equal(x => x.NewPassword).WithMessage(pSharedLocalizer["MustMatch", nameof(ChangePasswordDto.ConfirmPassword), nameof(ChangePasswordDto.NewPassword)]);
    }
}
