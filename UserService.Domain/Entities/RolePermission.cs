using UserService.Domain.Common.Entity;

namespace UserService.Domain.Entities;

public record RolePermission : SoftDeleteAuditEntity<Guid>
{
    public required Guid RoleId { get; set; }
    public required Guid PermissionId { get; set; }

    public virtual required Role Role { get; set; }
    public virtual required Permission Permission { get; set; }
}
