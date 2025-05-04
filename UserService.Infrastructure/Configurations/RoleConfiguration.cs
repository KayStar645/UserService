using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable(nameof(Role));
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code).HasMaxLength(200);
        builder.Property(x => x.Name).HasMaxLength(200);

        builder.Property(x => x.CompanyId).HasMaxLength(36);
        builder.Property(x => x.BranchId).HasMaxLength(36);

        builder.HasIndex(x => new { x.CompanyId, x.BranchId });
        builder.HasIndex(x => new { x.Code, x.CompanyId, x.BranchId }).IsUnique().HasFilter("\"IsRemoved\" = false");
    }
}