using CesiZen.Domain.Datamodel;
using CesiZen.Infrastructure.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.Bson;
using MongoDB.EntityFrameworkCore.Extensions;

namespace CesiZen.Infrastructure.Configuration;

public class LoginConfiguration : IEntityTypeConfiguration<Login>
{
    public void Configure(EntityTypeBuilder<Login> builder)
    {
        builder.ToCollection("logins");

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.Id)
            .IsUnique();

        builder.HasIndex(x => x.Email).IsUnique();

        builder.Property(u => u.Id)
        .HasConversion(
            id => ObjectId.Parse(id),
            id => id.ToString())
        .HasColumnType("objectId");

        builder.Property(u => u.Id)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<ObjectIdProvider>();

        builder.Property(x => x.Email)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.Password)
            .IsRequired();

        builder.Property(x => x.Salt)
            .IsRequired();


    }
}
