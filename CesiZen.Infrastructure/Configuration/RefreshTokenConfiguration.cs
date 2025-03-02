using CesiZen.Infrastructure.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.Bson;
using MongoDB.EntityFrameworkCore.Extensions;

namespace CesiZen.Infrastructure.Configuration;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToCollection("refreshTokens");

        builder.HasKey(u => u.Id);

        builder.HasIndex(u => u.Id);

        builder.HasIndex(x => x.Token)
            .IsUnique();

        builder.Property(u => u.Id)
        .HasConversion(
            id => ObjectId.Parse(id),
            id => id.ToString())
        .HasColumnType("objectId");

        builder.Property(u => u.Id)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<ObjectIdProvider>();
    }
}
