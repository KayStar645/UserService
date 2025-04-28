using UserService.Domain.Common.Entity;

namespace UserService.Domain.Entities;

public record Role : EntityAuditWithOrgDetail<Guid>
{
    public string? Code { get; set; }
    public string? Name { get; set; }

    public virtual required ICollection<Permission> Permissions { get; set; }
}
