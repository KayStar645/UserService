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
            if (entry.Entity is IAuditable auditableEntity)
            {
                string userName = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(static c => c.Type == CONSTANT_CLAIM_TYPES.USER)?.Value ?? string.Empty;
                string staffCode = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == CONSTANT_CLAIM_TYPES.STAFF)?.Value ?? string.Empty;
                if (entry.State == EntityState.Added)
                {
                    if(entry.Entity is IBaseEntity<Guid> entity)
                    {
                       entity.Id = Guid.NewGuid();
                    }    
                    auditableEntity.CreatedAt = DateTimeOffset.UtcNow;
                    auditableEntity.CreatedByUser = userName;
                    auditableEntity.CreatedByCode = staffCode;
                }
                else if (entry.State == EntityState.Modified)
                {
                    auditableEntity.LastModifiedAt = DateTimeOffset.UtcNow;
                    auditableEntity.ModifiedByUser = userName;
                    auditableEntity.ModifiedByCode = staffCode;
                }
                else if (entry.State == EntityState.Deleted && entry.Entity is ISoftDelete entity)
                {
                    entity.IsRemoved = true;
                    entry.State = EntityState.Unchanged;
                }
            }
        }

    }
}
