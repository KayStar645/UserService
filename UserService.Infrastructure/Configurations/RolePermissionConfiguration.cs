using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Configurations;

public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable(nameof(RolePermission));
        builder.HasKey(x => x.Id);

        builder.Property(x => x.IsRemoved).HasDefaultValue(false);
        builder.HasQueryFilter(x => !x.IsRemoved);

        builder.HasIndex(x => new { x.RoleId, x.PermissionId });
    }
}