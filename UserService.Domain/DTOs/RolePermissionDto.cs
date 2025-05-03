using UserService.Domain.Common.DTO;

namespace UserService.Domain.DTOs;

public record RolePermissionDto : SoftDeleteBaseDto<Guid>
{
    public required Guid PermissionId { get; set; }
    public required PermissionDto Permission { get; set; }
}

public abstract record CreateOrUpdateRolePermissionDto
{
    public required Guid PermissionId { get; set; }
}

public record CreateRolePermissionDto : CreateOrUpdateRolePermissionDto
{

}

public record UpdateRolePermissionDto : CreateOrUpdateRolePermissionDto
{

}