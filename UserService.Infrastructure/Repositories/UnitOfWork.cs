using Microsoft.EntityFrameworkCore.Storage;
using UserService.Domain.Common.Entity;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Infrastructure.Repositories;

public class UnitOfWork<TKey> : IUnitOfWork<TKey>
{
    private readonly UserDbContext _context;

    public UnitOfWork(UserDbContext context)
    {
        _context = context;
    }

    public IGenericRepository<TEntity, TKey> Set<TEntity>() where TEntity : BaseEntity<TKey>
    {
        return new GenericRepository<TEntity, TKey>(_context);
    }

    public async Task<TKey> SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
        return default!;
    }

    public async Task<TKey> SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
        return default!;
    }

    public async Task<TKey> SaveAndRemoveCache(CancellationToken cancellationToken, params string[] cacheKeys)
    {
        // Có thể thêm logic xóa cache tại đây nếu cần
        return await SaveChangesAsync(cancellationToken);
    }

    public async Task Rollback()
    {
        await _context.Database.RollbackTransactionAsync();
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
