using UserService.Domain.Enums;

namespace UserService.Application.DTOs;

public record UserDto
{
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
    public string? Email { get; set; }
    public bool IsEmailConfirmed { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IsPhoneNumberConfirmed { get; set; }
    public string? FullName { get; set; }
    public string? AvatarUrl { get; set; }
    public DateTimeOffset? DateOfBirth { get; set; }
    public EGender? Gender { get; set; }

    public required ICollection<RoleDto> Roles { get; set; }
    public required ICollection<PermissionDto> Permissions { get; set; }
}
