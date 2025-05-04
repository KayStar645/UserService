using UserService.Domain.Common.Entity.Interfaces;
using UserService.Domain.Common.Interfaces;
using UserService.Domain.Enums;

namespace UserService.Domain.Common.Entity;

public abstract record AuditWithOrgEntity<TKey> : AuditBaseEntity<TKey>, IOrganizationScope
{
    public string? CompanyId { get; set; }
    public string? BranchId { get; set; }
}

public abstract record SoftDeleteAuditableEntity<TKey> : AuditWithOrgEntity<TKey>, ISoftDelete
{
    public bool IsRemoved { get; set; } = false;
}

public abstract record StatusAuditableEntity<TKey> : AuditWithOrgEntity<TKey>, IStatus
{
    public EStatus Status { get; set; } = EStatus.Draft;
}

public abstract record ActiveAuditableEntity<TKey> : AuditWithOrgEntity<TKey>, IActivatable
{
    public bool IsActive { get; set; } = true;
}
