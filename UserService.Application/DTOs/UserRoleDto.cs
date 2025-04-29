using UserService.Domain.Common.DTO;

namespace UserService.Application.DTOs;

public record UserRoleDto : SoftDeleteBaseDto<Guid>
{
    public required Guid UserId { get; set; }
    public required Guid RoleId { get; set; }

    public required UserDto User { get; set; }
    public required RoleDto Role { get; set; }
}
