using UserService.Domain.Common.DTO;

namespace UserService.Domain.DTOs;

public record UserRoleDto : SoftDeleteBaseDto<Guid>
{
    public required Guid RoleId { get; set; }
    public required RoleDto Role { get; set; }
}

public abstract record CreateOrUpdateUserRoleDto
{
    public required Guid RoleId { get; set; }
}

public record CreateUserRoleDto : CreateOrUpdateUserRoleDto
{

}

public record UpdateUserRoleDto : CreateOrUpdateUserRoleDto
{

}
