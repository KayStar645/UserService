using UserService.Domain.Enums;

namespace UserService.Domain.Common.Entity.Interfaces;

public interface IBaseEntity<TKey>
{
    TKey? Id { get; set; }
}