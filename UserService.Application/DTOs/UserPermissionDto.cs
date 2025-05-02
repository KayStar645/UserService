using UserService.Domain.Common.DTO;

namespace UserService.Application.DTOs;

public record UserPermissionDto : SoftDeleteBaseDto<Guid>
{
    public required Guid PermissionId { get; set; }
    public required PermissionDto Permission { get; set; }
}

public abstract record CreateOrUpdateUserPermissionDto
{
    public required Guid PermissionId { get; set; }
}

public record CreateUserPermissionDto : CreateOrUpdateUserPermissionDto
{

}

public record UpdateUserPermissionDto : CreateOrUpdateUserPermissionDto
{

}