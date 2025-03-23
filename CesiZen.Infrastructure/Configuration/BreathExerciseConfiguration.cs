using CesiZen.Domain.Datamodel;
using CesiZen.Infrastructure.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CesiZen.Infrastructure.Configuration;

public class BreathExerciseConfiguration : IEntityTypeConfiguration<BreathExercise>
{
    public void Configure(EntityTypeBuilder<BreathExercise> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.Id)
            .IsUnique();

        builder.Property(u => u.Id)
            .ValueGeneratedOnAdd()
            .HasValueGenerator<ObjectIdProvider>();
    }
}
