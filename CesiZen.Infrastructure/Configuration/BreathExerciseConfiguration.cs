using CesiZen.Domain.Datamodel;
using CesiZen.Infrastructure.Providers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.Bson;
using MongoDB.EntityFrameworkCore.Extensions;

namespace CesiZen.Infrastructure.Configuration;

public class BreathExerciseConfiguration : IEntityTypeConfiguration<BreathExercise>
{
    public void Configure(EntityTypeBuilder<BreathExercise> builder)
    {
        builder.ToCollection("breathExercises");

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.Id)
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
