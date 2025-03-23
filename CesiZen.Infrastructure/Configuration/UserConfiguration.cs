using CesiZen.Domain.Datamodel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CesiZen.Infrastructure.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.HasIndex(u => u.Username);

        builder.Property(u => u.Firstname)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(u => u.Lastname)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(u => u.Username)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(u => u.CreatedAt)
            .IsRequired();

        builder.Property(u => u.IsActive)
            .IsRequired();

        builder.Property(u => u.Role);

        builder.HasOne(x => x.Login)
           .WithOne(x => x.User)
           .HasForeignKey<Login>(x => x.UserId)
           .IsRequired();

        builder.HasOne(x => x.Session)
            .WithOne(x => x.User)
            .HasForeignKey<Session>(x => x.UserId)
            .IsRequired();

        builder.HasOne(x => x.RefreshToken)
            .WithOne(x => x.User)
            .HasForeignKey<RefreshToken>(x => x.UserId)
            .IsRequired();
    }
}
