﻿using UserService.Domain.Common.Entity;
using UserService.Domain.Enums;

namespace UserService.Domain.Entities;

public record User : ActiveAuditableEntity<Guid>
{
    public required string Username { get; set; }
    public string? PasswordHash { get; set; }
    public string? Email { get; set; }
    public bool IsEmailConfirmed { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IsPhoneNumberConfirmed { get; set; }
    public string? FullName { get; set; }
    public string? AvatarUrl { get; set; }
    public DateTimeOffset? DateOfBirth { get; set; }
    public EGender? Gender { get; set; }

    public virtual required ICollection<UserRole> UserRoles { get; set; }
    public virtual required ICollection<UserPermission> UserPermissions { get; set; }
}
