using UserService.Domain.Common.Entity.Interfaces;

namespace UserService.Domain.Common.Entity;

public abstract record BaseEntity<TKey> : IBaseEntity<TKey>
{
    public TKey? Id { get; set; }

    public virtual void Update(BaseEntity<TKey> entity)
    {

    }
}
