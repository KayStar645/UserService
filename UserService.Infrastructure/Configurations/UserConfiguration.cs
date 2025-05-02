using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Entities;

namespace UserService.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(nameof(User));
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Username).HasMaxLength(36);
        builder.Property(x => x.PasswordHash).HasMaxLength(36);
        builder.Property(x => x.Email).HasMaxLength(50);
        builder.Property(x => x.PhoneNumber).HasMaxLength(15);
        builder.Property(x => x.FullName).HasMaxLength(190);
        builder.Property(x => x.AvatarUrl).HasMaxLength(190);

        builder.Property(x => x.IsEmailConfirmed).HasDefaultValue(false);
        builder.Property(x => x.IsPhoneNumberConfirmed).HasDefaultValue(false);

        builder.HasIndex(x => x.Username).IsUnique();
        builder.HasIndex(x => x.Email).IsUnique();
        builder.HasIndex(x => x.PhoneNumber).IsUnique();
    }
}
