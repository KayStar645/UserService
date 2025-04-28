using UserService.Domain.Enums;

namespace UserService.Domain.Common.Interfaces;

public interface IEntityBase<T>
{
    T Id { get; set; }
}

public interface IActivatable
{
    bool IsActive { get; set; }
}

public interface ISoftDelete
{
    bool IsRemoved { get; set; }
}

public interface IStatus
{
    EStatus Status { get; set; }
}