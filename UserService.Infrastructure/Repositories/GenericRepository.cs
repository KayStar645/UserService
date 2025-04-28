using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UserService.Domain.Common.Entity;
using UserService.Infrastructure.Repositories.Interfaces;

namespace UserService.Infrastructure.Repositories;

public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey>
    where TEntity : BaseEntity<TKey>
{
    protected readonly UserDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    public GenericRepository(UserDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public IQueryable<TEntity> Entities => _dbSet.AsQueryable();

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await _dbSet.AddRangeAsync(entities);
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.AnyAsync(predicate);
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        IQueryable<TEntity> query = AddInclude(_dbSet, includeProperties);
        return await query.AnyAsync(predicate);
    }

    public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate);
    }

    public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        IQueryable<TEntity> query = AddInclude(_dbSet, includeProperties);
        return await query.FirstOrDefaultAsync(predicate);
    }

    public IQueryable<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
    {
        return _dbSet.Where(predicate);
    }

    public IQueryable<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        return AddInclude(_dbSet.Where(predicate), includeProperties);
    }

    public IQueryable<TEntity> GetAll()
    {
        return _dbSet.AsQueryable();
    }

    public IQueryable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includeProperties)
    {
        return AddInclude(_dbSet, includeProperties);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includeProperties)
    {
        IQueryable<TEntity> query = AddInclude(_dbSet, includeProperties);
        return await query.ToListAsync();
    }

    public IQueryable<TEntity> GetByCondition(Expression<Func<TEntity, bool>> predicate)
    {
        return _dbSet.Where(predicate);
    }

    public IQueryable<TEntity> GetByCondition(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        return AddInclude(_dbSet.Where(predicate), includeProperties);
    }

    public async Task<IEnumerable<TEntity>> GetByConditionAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> GetByConditionAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        IQueryable<TEntity> query = AddInclude(_dbSet.Where(predicate), includeProperties);
        return await query.ToListAsync();
    }

    public IQueryable<TEntity> GetById(TKey id)
    {
        return _dbSet.Where(e => e.Id.Equals(id));
    }

    public IQueryable<TEntity> GetById(TKey id, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        return AddInclude(_dbSet.Where(e => e.Id.Equals(id)), includeProperties);
    }

    public async Task<TEntity> GetByIdAsync(TKey id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<TEntity> GetByIdAsync(TKey id, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        IQueryable<TEntity> query = AddInclude(_dbSet, includeProperties);
        return await query.FirstOrDefaultAsync(e => e.Id.Equals(id));
    }

    public void Remove(TEntity entity)
    {
        _dbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        _dbSet.RemoveRange(entities);
    }

    public TEntity Update(TEntity entity)
    {
        _dbSet.Update(entity);
        return entity;
    }

    public void UpdateRange(IEnumerable<TEntity> entities)
    {
        _dbSet.UpdateRange(entities);
    }

    public IQueryable<TEntity> AddInclude(IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        if (includeProperties != null)
        {
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
        }
        return query;
    }
}
