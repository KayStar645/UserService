using UserService.Domain.Common.DTO;

namespace UserService.Application.DTOs;

public record RolePermissionDto : SoftDeleteBaseDto<Guid>
{
    public required Guid RoleId { get; set; }
    public required Guid PermissionId { get; set; }

    public required RoleDto Role { get; set; }
    public required PermissionDto Permission { get; set; }
}
