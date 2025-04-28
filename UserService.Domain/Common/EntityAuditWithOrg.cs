using System.ComponentModel.DataAnnotations.Schema;
using UserService.Domain.Common.Interfaces;
using UserService.Domain.Enums;

namespace UserService.Domain.Common;

public abstract record EntityAuditWithOrg<TKey> : EntityAuditBase<TKey>
{
    [Column(TypeName = "VARCHAR(36)")]
    public string? CompanyId { get; set; }

    [Column(TypeName = "VARCHAR(36)")]
    public string? BranchId { get; set; }
}

public abstract record EntityAuditWithOrgDetail<TKey> : EntityAuditWithOrg<TKey>
{
    public bool IsRemoved { get; set; } = false;
}

public abstract record EntityAuditWithOrgMasterData<TKey> : EntityAuditWithOrg<TKey>, IActivatable
{
    public bool IsActive { get; set; } = true;
}

public abstract record EntityAuditWithOrgDocument<TKey> : EntityAuditWithOrg<TKey>
{
    public EStatus Status { get; set; } = EStatus.Draft;
}
