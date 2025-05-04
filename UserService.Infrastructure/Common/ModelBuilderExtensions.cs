using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq.Expressions;
using UserService.Domain.Common.Constants;
using UserService.Domain.Common.Entity.Interfaces;

namespace UserService.Infrastructure.Common;

public static class ModelBuilderExtensions
{
    public static void ConfigureCommonColumn(this ModelBuilder modelBuilder)
    {
        ConfigureFilterIsRemovedField(modelBuilder);
    }

    private static void ConfigureFilterIsRemovedField(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var clrType = entityType.ClrType;

            // Bỏ qua entity không có property IsRemoved
            if (clrType.GetProperty("IsRemoved") == null)
                continue;

            // Lấy EntityTypeBuilder<TEntity>
            var entityMethod = typeof(ModelBuilder)
                .GetMethod(nameof(ModelBuilder.Entity), Array.Empty<Type>())
                ?.MakeGenericMethod(clrType);

            var entityBuilder = entityMethod?.Invoke(modelBuilder, null);
            if (entityBuilder == null) continue;

            // Gọi Property(x => x.IsRemoved)
            var propertyMethod = entityBuilder.GetType().GetMethods()
                .FirstOrDefault(m => m.Name == "Property" && m.GetParameters().Length == 1);

            var isRemovedProp = clrType.GetProperty("IsRemoved")!;
            var parameter = Expression.Parameter(clrType, "x");
            var propertyAccess = Expression.Property(parameter, isRemovedProp);
            var lambda = Expression.Lambda(propertyAccess, parameter);

            var genericPropertyMethod = propertyMethod?.MakeGenericMethod(typeof(bool));
            var propertyBuilder = genericPropertyMethod?.Invoke(entityBuilder, new object[] { lambda });

            // Gọi HasDefaultValue(false)
            var hasDefaultValueMethod = propertyBuilder?.GetType()
                .GetMethod("HasDefaultValue", new[] { typeof(bool) });
            hasDefaultValueMethod?.Invoke(propertyBuilder, new object[] { false });

            // Gọi HasQueryFilter(x => !x.IsRemoved)
            var isRemovedExpr = Expression.Property(parameter, "IsRemoved");
            var notRemovedExpr = Expression.Not(isRemovedExpr);
            var filterLambda = Expression.Lambda(notRemovedExpr, parameter);

            var hasQueryFilterMethod = entityBuilder.GetType()
                .GetMethods()
                .First(m =>
                    m.Name == "HasQueryFilter"
                    && m.GetParameters().Length == 1
                    && m.GetParameters()[0].ParameterType == typeof(LambdaExpression)
                );
            hasQueryFilterMethod.Invoke(entityBuilder, new object[] { filterLambda });

        }
    }

    public static void ConfigureSharedColumnOrder(this ModelBuilder modelBuilder)
    {
        var auditFields = new[]
        {
            "Status", "IsActive", "IsRemoved",
            "CreatedByCode", "CreatedByUser", "CreatedAt",
            "LastModifiedByCode", "LastModifiedByUser", "LastModifiedAt",
        };

        var maxLengthFields = new Dictionary<string, int>
        {
            { "CreatedByCode", FieldLengthConstants.CodeMaxLength },
            { "CreatedByUser", FieldLengthConstants.IdMaxLength },
            { "LastModifiedByCode", FieldLengthConstants.CodeMaxLength },
            { "LastModifiedByUser", FieldLengthConstants.IdMaxLength }
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
                if (maxLengthFields.TryGetValue(name, out var maxLength))
                {
                    p.SetMaxLength(maxLength);
                }
            }
        }
    }

    public static void ConfigureCompanyBranchIndex<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : class, IOrganizationScope
    {
        builder.Property(x => x.CompanyId).HasMaxLength(36);
        builder.Property(x => x.CompanyId).HasMaxLength(36);
        builder.HasIndex(x => new { x.CompanyId, x.BranchId });
    }
}

