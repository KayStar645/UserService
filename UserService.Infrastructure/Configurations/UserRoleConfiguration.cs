using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Configurations;

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable(nameof(UserRole));
        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId).HasMaxLength(36);
        builder.Property(x => x.RoleId).HasMaxLength(36);

        builder.HasIndex(x => new { x.UserId, x.RoleId });
    }
}