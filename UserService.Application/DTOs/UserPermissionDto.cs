using UserService.Domain.Common.DTO;

namespace UserService.Application.DTOs;

public record UserPermissionDto : SoftDeleteBaseDto<Guid>
{
    public required Guid UserId { get; set; }
    public required Guid PermissionId { get; set; }

    public required UserDto User { get; set; }
    public required PermissionDto Permission { get; set; }
}
