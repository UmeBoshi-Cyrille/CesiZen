using CesiZen.Domain.Datamodel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.EntityFrameworkCore.Extensions;

namespace CesiZen.Infrastructure.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToCollection("users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Username).IsRequired();
        builder.Property(u => u.Email).IsRequired();
        builder.Property(u => u.PasswordHash).IsRequired();
        builder.Property(u => u.Salt).IsRequired();
        builder.Property(u => u.CreatedAt).IsRequired();
        builder.Property(u => u.IsActive).IsRequired();
        builder.Property(u => u.Role);

        builder.HasIndex(u => u.Email).IsUnique();
        builder.HasIndex(u => u.Username);

    }
}
