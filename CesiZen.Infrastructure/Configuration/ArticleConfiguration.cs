using CesiZen.Domain.Datamodel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CesiZen.Infrastructure.Configuration;

public class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.Id)
            .IsUnique();

        builder.HasMany(x => x.Categories)
            .WithMany(x => x.Articles);

        builder.HasMany(x => x.Images)
            .WithOne(x => x.Article)
            .HasForeignKey(x => x.ArticleId);
    }
}
