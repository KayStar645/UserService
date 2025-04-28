using MediatR;

namespace UserService.Domain.Events.FireAndForget;

public class DeletedEventFireAndForget<TRequest, TEntity> : INotification
{
    public TRequest Request { get; }
    public TEntity Entity { get; }

    public DeletedEventFireAndForget(TRequest request, TEntity entity)
    {
        Request = request;
        Entity = entity;
    }
}