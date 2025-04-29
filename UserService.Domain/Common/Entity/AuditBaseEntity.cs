using System.ComponentModel.DataAnnotations.Schema;
using UserService.Domain.Common.Entity.Interfaces;
using UserService.Domain.Common.Interfaces;
using UserService.Domain.Enums;

namespace UserService.Domain.Common.Entity;

public abstract record AuditBaseEntity<TKey> : BaseEntity<TKey>, IAuditable
{
    [Column(TypeName = "VARCHAR(36)")]
    public string? CreatedByCode { get; set; }

    [Column(TypeName = "VARCHAR(36)")]
    public string? ModifiedByCode { get; set; }
    [Column(TypeName = "VARCHAR(36)")]
    public string? CreatedByUser { get; set; }
    [Column(TypeName = "VARCHAR(36)")]
    public string? ModifiedByUser { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? LastModifiedAt { get; set; }
}

public abstract record SoftDeleteAuditEntity<TKey> : AuditBaseEntity<TKey>, ISoftDelete
{
    public bool IsRemoved { get; set; } = false;
}

public abstract record StatusAuditEntity<TKey> : AuditBaseEntity<TKey>
{
    public EStatus Status { get; set; } = EStatus.Draft;
}

public abstract record ActiveAuditEntity<TKey> : AuditBaseEntity<TKey>, IActivatable
{
    public bool IsActive { get; set; } = true;
}