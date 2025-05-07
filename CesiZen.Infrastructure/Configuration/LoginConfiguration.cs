using CesiZen.Domain.Datamodel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CesiZen.Infrastructure.Configuration;

public class LoginConfiguration : IEntityTypeConfiguration<Login>
{
    public void Configure(EntityTypeBuilder<Login> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.Id)
            .IsUnique();

        builder.Property(u => u.Id)
            .ValueGeneratedOnAdd();

        builder.HasIndex(x => x.Email).IsUnique();

        builder.Property(x => x.Email)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.Password)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.Salt)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.EmailVerificationToken)
            .HasMaxLength(255);

        builder.Property(x => x.PasswordResetToken)
            .HasMaxLength(255);

        builder.HasMany(x => x.ResetPasswords)
            .WithOne(x => x.Login)
            .HasForeignKey(x => x.LoginId);
    }
}
