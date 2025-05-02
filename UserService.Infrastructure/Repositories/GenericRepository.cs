using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using UserService.Domain.Common.Entity;
using UserService.Infrastructure.Persistence;
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

    // Lấy tất cả hoặc lấy có điều kiện
    public IQueryable<TEntity> GetAll() => _dbSet.AsQueryable();
    public IQueryable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includeProperties) => AddInclude(_dbSet, includeProperties);
    public async Task<IEnumerable<TEntity>> GetAllAsync() => await _dbSet.ToListAsync();
    public async Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includeProperties) => await AddInclude(_dbSet, includeProperties).ToListAsync();
    public IQueryable<TEntity> GetByCondition(Expression<Func<TEntity, bool>> predicate) => _dbSet.Where(predicate);
    public IQueryable<TEntity> GetByCondition(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties) => AddInclude(_dbSet.Where(predicate), includeProperties);
    public async Task<IEnumerable<TEntity>> GetByConditionAsync(Expression<Func<TEntity, bool>> predicate) => await _dbSet.Where(predicate).ToListAsync();
    public async Task<IEnumerable<TEntity>> GetByConditionAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties) => await AddInclude(_dbSet.Where(predicate), includeProperties).ToListAsync();

    // Lấy 1 phần tử theo id hoặc theo điều kiện
    public async Task<TEntity?> GetByIdAsync(TKey id) => await _dbSet.FindAsync(id);
    public async Task<TEntity?> GetByIdAsync(TKey id, params Expression<Func<TEntity, object>>[] includeProperties) => await AddInclude(_dbSet, includeProperties).FirstOrDefaultAsync(e => e.Id.Equals(id));
    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate) => await _dbSet.FirstOrDefaultAsync(predicate);
    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties) => await AddInclude(_dbSet, includeProperties).FirstOrDefaultAsync(predicate);

    public IQueryable<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> predicate) => _dbSet.Where(predicate);
    public IQueryable<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties) => AddInclude(_dbSet.Where(predicate), includeProperties);
    public IQueryable<TEntity> GetById(TKey id) => _dbSet.Where(e => e.Id.Equals(id));
    public IQueryable<TEntity> GetById(TKey id, params Expression<Func<TEntity, object>>[] includeProperties) => AddInclude(_dbSet.Where(e => e.Id.Equals(id)), includeProperties);


    // Kiểm tra sự tồn tại
    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate) => await _dbSet.AnyAsync(predicate);
    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties) => await AddInclude(_dbSet, includeProperties).AnyAsync(predicate);

    // Thêm dữ liệu
    public async Task AddAsync(TEntity entity) => await _dbSet.AddAsync(entity);
    public async Task AddRangeAsync(IEnumerable<TEntity> entities) => await _dbSet.AddRangeAsync(entities);

    // Cập nhật dữ liệu
    public void Update(TEntity entity) => _dbSet.Update(entity);
    public void UpdateRange(IEnumerable<TEntity> entities) => _dbSet.UpdateRange(entities);

    // Xóa dữ liệu
    public void Remove(TEntity entity) => _dbSet.Remove(entity);
    public void RemoveRange(IEnumerable<TEntity> entities) => _dbSet.RemoveRange(entities);

    // Include

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

    public IQueryable<TEntity> Entities => _dbSet.AsQueryable();
   
}
