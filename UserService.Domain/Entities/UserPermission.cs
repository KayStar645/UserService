using UserService.Domain.Common.Entity;

namespace UserService.Domain.Entities;

public record UserPermission : EntityDetailAudit<Guid>
{
    public required string UserId { get; set; }
    public required string PermissionId { get; set; }
    public virtual required User User { get; set; }
    public virtual required Permission Permission { get; set; }
}
