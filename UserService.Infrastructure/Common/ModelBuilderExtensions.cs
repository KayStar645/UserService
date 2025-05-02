using Microsoft.EntityFrameworkCore;

namespace UserService.Infrastructure.Common;

public static class ModelBuilderExtensions
{
    public static void ConfigureSharedColumnOrder(this ModelBuilder modelBuilder)
    {
        var auditFields = new[]
        {
            "Status", "IsActive", "IsRemoved",
            "CreatedByCode", "CreatedByUser", "CreatedAt",
            "LastModifiedByCode", "LastModifiedByUser", "LastModifiedAt",
        };

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var props = entityType.GetProperties().ToList();

            int order = 0;
            foreach (var field in new[] { "Id", "CompanyId", "BranchId" })
            {
                var p = entityType.FindProperty(field);
                if (p != null)
                {
                    p.SetColumnOrder(order++);
                }
            }

            foreach (var name in auditFields)
            {
                var p = entityType.FindProperty(name);
                if (p != null)
                {
                    p.SetColumnOrder(order++);
                }
            }
        }
    }
}

