using MediatR;

namespace UserService.Domain.Events.FireAndForget;

public class UpdatedEventFireAndForget<TRequest, TEntity> : INotification
{
    public TRequest Request { get; }
    public TEntity OldEntity { get; }
    public TEntity Entity { get; }

    public UpdatedEventFireAndForget(TRequest request, TEntity oldEntity, TEntity entity)
    {
        Request = request;
        OldEntity = oldEntity;
        Entity = entity;
    }
}
