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

public record ChangePasswordPasswordDto : UpdateCommandDto<Guid, UserDto>
{
    public string? OldPassword { get; set; }
    public string? NewPassword { get; set; }
    public string? ConfirmPassword { get; set; }
}

public class ChangePasswordValidator : UpdateCommandValidator<Guid, ChangePasswordPasswordDto, UserDto>
{
    public ChangePasswordValidator(IUnitOfWork<Guid> pUnitOfWork, IStringLocalizer<SharedResource> pSharedLocalizer) : base(pUnitOfWork, pSharedLocalizer)
    {
        RuleFor(x => x.OldPassword)
            .NotEmpty().WithMessage(pSharedLocalizer["Required", nameof(ChangePasswordPasswordDto.OldPassword)])
            .MinimumLength(FieldLengthConstants.User.PasswordMinLength).WithMessage(pSharedLocalizer["MinimumLength", nameof(ChangePasswordPasswordDto.OldPassword), FieldLengthConstants.User.PasswordMinLength])
            .MaximumLength(FieldLengthConstants.User.PasswordMaxLength).WithMessage(pSharedLocalizer["MaximumLength", nameof(ChangePasswordPasswordDto.OldPassword), FieldLengthConstants.User.PasswordMaxLength]);

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage(pSharedLocalizer["Required", nameof(ChangePasswordPasswordDto.NewPassword)])
            .MinimumLength(FieldLengthConstants.User.PasswordMinLength).WithMessage(pSharedLocalizer["MinimumLength", nameof(ChangePasswordPasswordDto.NewPassword), FieldLengthConstants.User.PasswordMinLength])
            .MaximumLength(FieldLengthConstants.User.PasswordMaxLength).WithMessage(pSharedLocalizer["MaximumLength", nameof(ChangePasswordPasswordDto.NewPassword), FieldLengthConstants.User.PasswordMaxLength])
            .Must((dto, newPassword) => newPassword != dto.OldPassword)
            .WithMessage(pSharedLocalizer["MustNotEqual", nameof(ChangePasswordPasswordDto.NewPassword), nameof(ChangePasswordPasswordDto.OldPassword)]);

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage(pSharedLocalizer["Required", nameof(ChangePasswordPasswordDto.ConfirmPassword)])
            .Equal(x => x.NewPassword).WithMessage(pSharedLocalizer["MustMatch", nameof(ChangePasswordPasswordDto.ConfirmPassword), nameof(ChangePasswordPasswordDto.NewPassword)]);
    }
}
