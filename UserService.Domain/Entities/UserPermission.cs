using UserService.Domain.Common.Entity;

namespace UserService.Domain.Entities;

public record UserPermission : SoftDeleteAuditEntity<Guid>
{
    public required Guid UserId { get; set; }
    public required Guid PermissionId { get; set; }
    public virtual required User User { get; set; }
    public virtual required Permission Permission { get; set; }
}
