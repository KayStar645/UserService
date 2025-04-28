using MediatR;

namespace UserService.Domain.Events.Async;

public class DeletedEventAsync<TRequest, TEntity> : INotification
{
    public TRequest Request { get; }
    public TEntity Entity { get; }

    public DeletedEventAsync(TRequest request, TEntity entity)
    {
        Request = request;
        Entity = entity;
    }
}
