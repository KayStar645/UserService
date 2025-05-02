using UserService.Domain.Common.Entity.Interfaces;
using UserService.Domain.Common.Interfaces;
using UserService.Domain.Enums;

namespace UserService.Domain.Common.DTO;

public record BaseWithOrgDto<TKey> : BaseDto<TKey>, IOrganizationScope
{
    public string? CompanyId { get; set; }
    public string? BranchId { get; set; }
}

public abstract record StatusBaseWithOrgDto<TKey> : BaseWithOrgDto<TKey>, IStatus
{
    public EStatus Status { get; set; } = EStatus.Draft;
}

public abstract record ActiveBaseWithOrgDto<TKey> : BaseWithOrgDto<TKey>, IActivatable
{
    public bool IsActive { get; set; } = true;
}
