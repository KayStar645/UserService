using UserService.Domain.Common.Entity;

namespace UserService.Domain.Entities;

public record UserRole : SoftDeleteAuditEntity<Guid>
{
    public required Guid UserId { get; set; }
    public required Guid RoleId { get; set; }

    public virtual required User User { get; set; }
    public virtual required Role Role { get; set; }
}
