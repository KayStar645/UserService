using UserService.Domain.Common;
using UserService.Domain.Enums;

namespace UserService.Domain.Entities;

public record User : EntityAuditWithOrgMasterData<Guid>
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

    public virtual required ICollection<Role> Roles { get; set; }
    public virtual required ICollection<Permission> Permissions { get; set; }
}
