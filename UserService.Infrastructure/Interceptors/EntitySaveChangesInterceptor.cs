using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using UserService.Domain.Common.Entity.Interfaces;
using UserService.Domain.Common.Interfaces;
using UserService.Infrastructure.Constants;

namespace UserService.Infrastructure.Interceptors;

public class EntitySaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public EntitySaveChangesInterceptor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext? context)
    {
        if (context is null)
            return;

        foreach (var entry in context.ChangeTracker.Entries())
        {
            string userName = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(static c => c.Type == CONSTANT_CLAIM_TYPES.USER)?.Value ?? string.Empty;
            string staffCode = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == CONSTANT_CLAIM_TYPES.STAFF)?.Value ?? string.Empty;

            // Dữ liệu không bao giờ được người dùng xóa khỏi hệ thống
            switch (entry.State)
            {
                case EntityState.Added:
                    if (entry.Entity is IBaseEntity<Guid> createEntity)
                        createEntity.Id = Guid.NewGuid();

                    if (entry.Entity is IAuditable addedEntity)
                    {
                        addedEntity.CreatedAt = DateTimeOffset.UtcNow;
                        addedEntity.CreatedByUser = userName;
                        addedEntity.CreatedByCode = staffCode;
                    }
                    break;
                case EntityState.Modified:
                    if (entry.Entity is IAuditable modifiedEntity)
                    {
                        modifiedEntity.LastModifiedAt = DateTimeOffset.UtcNow;
                        modifiedEntity.LastModifiedByUser = userName;
                        modifiedEntity.LastModifiedByCode = staffCode;
                    }
                    break;
                case EntityState.Deleted:
                    entry.State = EntityState.Unchanged;

                    if (entry.Entity is ISoftDelete deleteEntity)
                        deleteEntity.IsRemoved = true;
                    break;
            }
        }
    }
}
