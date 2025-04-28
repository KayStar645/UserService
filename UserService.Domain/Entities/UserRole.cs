using UserService.Domain.Common.Entity;

namespace UserService.Domain.Entities;

public record UserRole : EntityDetailAudit<Guid>
{
    public required string UserId { get; set; }
    public required string RoleId { get; set; }

    public virtual required User User { get; set; }
    public virtual required Role Role { get; set; }
}
