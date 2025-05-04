using FluentValidation;
using Microsoft.Extensions.Localization;
using UserService.Domain.DTOs;
using UserService.Application.Features.Base.Commands;
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


public class UpdateUserValidator : AbstractValidator<UpdateUserDto>
{
    public UpdateUserValidator(IUnitOfWork<Guid> pUnitOfWork, IStringLocalizer pSharedLocalizer)
    {
        RuleFor(x => x)
            .NotNull().WithMessage(pSharedLocalizer["ValidationError"]);
    }
}