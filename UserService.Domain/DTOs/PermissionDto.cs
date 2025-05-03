using UserService.Domain.Common.DTO;

namespace UserService.Domain.DTOs;

public record PermissionDto : BaseWithOrgDto<Guid>
{
    public string? Code { get; set; }
    public string? Name { get; set; }
}
