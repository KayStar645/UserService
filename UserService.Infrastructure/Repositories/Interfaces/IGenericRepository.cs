using System.Linq.Expressions;
using UserService.Domain.Common.Entity;

namespace UserService.Infrastructure.Repositories.Interfaces;

public interface IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
{
    // Lấy tất cả hoặc lấy có điều kiện
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includeProperties);
    Task<IEnumerable<TEntity>> GetByConditionAsync(Expression<Func<TEntity, bool>> predicate);
    Task<IEnumerable<TEntity>> GetByConditionAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);

    IQueryable<TEntity> GetAll();
    IQueryable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includeProperties);
    IQueryable<TEntity> GetByCondition(Expression<Func<TEntity, bool>> predicate);
    IQueryable<TEntity> GetByCondition(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);


    // Lấy 1 phần tử theo id hoặc theo điều kiện
    Task<TEntity?> GetByIdAsync(TKey id);
    Task<TEntity?> GetByIdAsync(TKey id, params Expression<Func<TEntity, object>>[] includeProperties);
    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);

    IQueryable<TEntity> GetById(TKey id);
    IQueryable<TEntity> GetById(TKey id, params Expression<Func<TEntity, object>>[] includeProperties);
    IQueryable<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
    IQueryable<TEntity> FirstOrDefault(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);


    // Kiểm tra sự tồn tại
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);


    // Thêm dữ liệu
    Task AddAsync(TEntity entity);
    Task AddRangeAsync(IEnumerable<TEntity> entities);


    // Cập nhật dữ liệu
    void Update(TEntity entity);
    void UpdateRange(IEnumerable<TEntity> entities);


    // Xóa dữ liệu
    void Remove(TEntity entity);
    void RemoveRange(IEnumerable<TEntity> entities);


    // Include
    IQueryable<TEntity> Entities { get; }
    IQueryable<TEntity> AddInclude(IQueryable<TEntity> query, Expression<Func<TEntity, object>>[] includeProperties);
}
