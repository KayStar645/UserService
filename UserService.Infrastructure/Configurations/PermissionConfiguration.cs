using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Common.Entity.Interfaces;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Configurations;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable(nameof(Permission));
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code).HasMaxLength(200);
        builder.Property(x => x.Name).HasMaxLength(200);

        builder.Property(x => x.CompanyId).HasMaxLength(36);
        builder.Property(x => x.BranchId).HasMaxLength(36);

        builder.Property(x => x.IsRemoved).HasDefaultValue(false);
        builder.HasQueryFilter(x => !x.IsRemoved);

        builder.HasIndex(x => new { x.CompanyId, x.BranchId });
        builder.HasIndex(x => new { x.Code, x.CompanyId, x.BranchId }).IsUnique();

        int order = 1000;
        builder.Property(x => x.CreatedByCode).HasColumnOrder(order++);
        builder.Property(x => x.CreatedByUser).HasColumnOrder(order++);
        builder.Property(x => x.CreatedAt).HasColumnOrder(order++);
        builder.Property(x => x.LastModifiedByCode).HasColumnOrder(order++);
        builder.Property(x => x.LastModifiedByUser).HasColumnOrder(order++);
        builder.Property(x => x.LastModifiedAt).HasColumnOrder(order++);
        builder.Property(x => x.IsRemoved).HasColumnOrder(order++);
    }
}