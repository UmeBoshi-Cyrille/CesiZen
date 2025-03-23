using CesiZen.Domain.Datamodel;
using CesiZen.Infrastructure.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CesiZen.Infrastructure.Configuration;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.Id)
            .IsUnique();

        builder.HasIndex(x => x.Name).IsUnique();

        builder.Property(u => u.Id)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<ObjectIdProvider>();
    }
}
