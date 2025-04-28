using MediatR;

namespace UserService.Domain.Events.Async;

public class UpdatedEventAsync<TRequest, TEntity> : INotification
{
    public TRequest Request { get; }
    public TEntity OldEntity { get; }
    public TEntity Entity { get; }

    public UpdatedEventAsync(TRequest request, TEntity oldEntity, TEntity entity)
    {
        Request = request;
        OldEntity = oldEntity;
        Entity = entity;
    }
}
