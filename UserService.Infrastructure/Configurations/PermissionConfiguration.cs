using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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

        builder.HasIndex(x => new { x.CompanyId, x.BranchId });
    }
}