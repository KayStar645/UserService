using System.ComponentModel.DataAnnotations.Schema;
using UserService.Domain.Common.Interfaces;
using UserService.Domain.Enums;

namespace UserService.Domain.Common;

public abstract record EntityAuditBase<TKey> : EntityBase<TKey>, IAuditable
{

    [Column(TypeName = "VARCHAR(36)")]
    public string? CreatedByCode { get; set; }

    [Column(TypeName = "VARCHAR(36)")]
    public string? ModifiedByCode { get; set; }
    [Column(TypeName = "VARCHAR(36)")]
    public string? CreatedByUser { get; set; }
    [Column(TypeName = "VARCHAR(36)")]
    public string? ModifiedByUser { get; set; }
    public DateTimeOffset? CreatedDate { get; set; }
    public DateTimeOffset? LastModifiedDate { get; set; }
}

public abstract record EntityDetailAudit<TKey> : EntityAuditBase<TKey>, ISoftDelete
{
    public bool IsRemoved { get; set; } = false;
}

public abstract record EntityDocumentAudit<TKey> : EntityAuditBase<TKey>
{
    public EStatus Status { get; set; } = EStatus.Draft;
}

public abstract record EntityMasterDataAudit<TKey> : EntityAuditBase<TKey>, IActivatable
{
    public bool IsActive { get; set; } = true;
}