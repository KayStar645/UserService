using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserService.Domain.Common.Constants;
using UserService.Domain.Entities;
using UserService.Infrastructure.Common;

namespace UserService.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(nameof(User));
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Username).HasMaxLength(FieldLengthConstants.User.UsernameMaxLength);
        builder.Property(x => x.PasswordHash).HasMaxLength(FieldLengthConstants.User.PasswordHashMaxLength);
        builder.Property(x => x.Email).HasMaxLength(FieldLengthConstants.EmailMaxLength);
        builder.Property(x => x.PhoneNumber).HasMaxLength(FieldLengthConstants.PhoneNumberMaxLength);
        builder.Property(x => x.FullName).HasMaxLength(FieldLengthConstants.User.FullNameMaxLength);
        builder.Property(x => x.AvatarUrl).HasMaxLength(FieldLengthConstants.UrlMaxLength);

        builder.Property(x => x.IsEmailConfirmed).HasDefaultValue(false);
        builder.Property(x => x.IsPhoneNumberConfirmed).HasDefaultValue(false);

        builder.HasIndex(x => x.Username).IsUnique();
        builder.HasIndex(x => x.Email).IsUnique();
        builder.HasIndex(x => x.PhoneNumber).IsUnique();

        builder.ConfigureCompanyBranchIndex();
    }
}
