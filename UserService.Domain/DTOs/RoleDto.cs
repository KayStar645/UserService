using UserService.Domain.Common.DTO;

namespace UserService.Domain.DTOs;

public record RoleDto : BaseWithOrgDto<Guid>
{
    public string? Code { get; set; }
    public string? Name { get; set; }

    public required IEnumerable<RolePermissionDto> RolePermissions { get; set; }
}
