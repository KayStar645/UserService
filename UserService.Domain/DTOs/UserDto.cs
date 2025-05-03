using UserService.Domain.Common.DTO;
using UserService.Domain.Enums;

namespace UserService.Domain.DTOs;

public record UserDto : ActiveBaseWithOrgDto<Guid>
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

    public required IEnumerable<RoleDto> Roles { get; set; }
    public required IEnumerable<PermissionDto> Permissions { get; set; }
}
