using Microsoft.EntityFrameworkCore.Storage;
using UserService.Domain.Common.Entity;

namespace UserService.Infrastructure.Repositories.Interfaces;

public interface IUnitOfWork<TKey> : IDisposable
{
    IGenericRepository<TEntity, TKey> Set<TEntity>() where TEntity : BaseEntity<TKey>;
    Task<TKey> SaveChangesAsync();

    Task<TKey> SaveChangesAsync(CancellationToken cancellationToken);

    Task<TKey> SaveAndRemoveCache(CancellationToken cancellationToken, params string[] cacheKeys);

    Task Rollback();
    Task<IDbContextTransaction> BeginTransactionAsync();
}
