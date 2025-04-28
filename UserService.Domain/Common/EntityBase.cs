using UserService.Domain.Common.Interfaces;

namespace UserService.Domain.Common;

public abstract record EntityBase<TKey> : IEntityBase<TKey>
{
    public TKey Id { get; set; }
}
