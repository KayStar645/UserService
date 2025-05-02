using UserService.Domain.Common.DTO;

namespace UserService.Application.DTOs;

public record RoleDto : BaseWithOrgDto<Guid>
{
    public string? Code { get; set; }
    public string? Name { get; set; }

    public required IEnumerable<PermissionDto> Permissions { get; set; }
}
