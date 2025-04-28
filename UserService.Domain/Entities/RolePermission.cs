using UserService.Domain.Common;

namespace UserService.Domain.Entities;

public record RolePermission : EntityDetailAudit<Guid>
{
    public required string RoleId { get; set; }
    public required string PermissionId { get; set; }

    public virtual required Role Role { get; set; }
    public virtual required Permission Permission { get; set; }
}
