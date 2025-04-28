using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Configurations;

public class UserPermissionConfiguration : IEntityTypeConfiguration<UserPermission>
{
    public void Configure(EntityTypeBuilder<UserPermission> builder)
    {
        builder.ToTable(nameof(UserPermission));
        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId).HasMaxLength(36);
        builder.Property(x => x.PermissionId).HasMaxLength(36);

        builder.HasIndex(x => new { x.UserId, x.PermissionId });
    }
}