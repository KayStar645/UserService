using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Common.Constants;
using UserService.Domain.Entities;
using UserService.Infrastructure.Common;

namespace UserService.Infrastructure.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable(nameof(Role));
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code).HasMaxLength(FieldLengthConstants.CodeMaxLength);
        builder.Property(x => x.Name).HasMaxLength(FieldLengthConstants.NameMaxLength);

        builder.HasIndex(x => new { x.Code, x.CompanyId, x.BranchId }).IsUnique().HasFilter("\"IsRemoved\" = false");

        builder.ConfigureCompanyBranchIndex();
    }
}