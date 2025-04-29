using UserService.Domain.Common.DTO;

namespace UserService.Application.DTOs;

public record PermissionDto : SoftDeleteBaseWithOrgDto<Guid>
{
    public string? Code { get; set; }
    public string? Name { get; set; }
}
