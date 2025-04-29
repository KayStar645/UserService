using UserService.Domain.Common.DTO.Interfaces;
using UserService.Domain.Common.Interfaces;
using UserService.Domain.Enums;

namespace UserService.Domain.Common.DTO;

public abstract record BaseDto<TKey> : IBaseDto<TKey>
{
    public TKey? Id { get; set; }
    public string? CreatedByCode { get; set; }
    public string? ModifiedByCode { get; set; }
    public string? CreatedByUser { get; set; }
    public string? ModifiedByUser { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? LastModifiedAt { get; set; }
}

public abstract record SoftDeleteBaseDto<TKey> : BaseDto<TKey>, ISoftDelete
{
    public bool IsRemoved { get; set; } = false;
}

public abstract record StatusBaseDto<TKey> : BaseDto<TKey>
{
    public EStatus Status { get; set; } = EStatus.Draft;
}

public abstract record ActiveBaseDto<TKey> : BaseDto<TKey>, IActivatable
{
    public bool IsActive { get; set; } = true;
}
