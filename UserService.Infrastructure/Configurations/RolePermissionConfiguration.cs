using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Configurations;

public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        builder.ToTable(nameof(RolePermission));
        builder.HasKey(x => x.Id);

        builder.Property(x => x.RoleId).HasMaxLength(36);
        builder.Property(x => x.PermissionId).HasMaxLength(36);

        builder.HasIndex(x => new { x.RoleId, x.PermissionId });
    }
}