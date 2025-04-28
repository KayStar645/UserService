using MediatR;

namespace UserService.Domain.Events.Async;

public class CreatedEventAsync<TRequest, TEntity> : INotification
{
    public TRequest Request { get; }
    public TEntity Entity { get; }

    public CreatedEventAsync(TRequest request, TEntity entity)
    {
        Request = request;
        Entity = entity;
    }
}

