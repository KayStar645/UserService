using MediatR;

namespace UserService.Domain.Events.FireAndForget;

public class CreatedEventFireAndForget<TRequest, TEntity> : INotification
{
    public TRequest Request { get; }
    public TEntity Entity { get; }

    public CreatedEventFireAndForget(TRequest request, TEntity entity)
    {
        Request = request;
        Entity = entity;
    }
}
