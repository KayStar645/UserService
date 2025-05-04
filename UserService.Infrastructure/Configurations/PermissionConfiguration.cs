using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Common.Constants;
using UserService.Domain.Entities;
using UserService.Infrastructure.Common;

namespace UserService.Infrastructure.Configurations;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable(nameof(Permission));
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code).HasMaxLength(FieldLengthConstants.CodeMaxLength);
        builder.Property(x => x.Name).HasMaxLength(FieldLengthConstants.NameMaxLength);

        builder.HasIndex(x => new { x.Code, x.CompanyId, x.BranchId }).IsUnique().HasFilter("\"IsRemoved\" = false");
        builder.ConfigureCompanyBranchIndex();
    }
}