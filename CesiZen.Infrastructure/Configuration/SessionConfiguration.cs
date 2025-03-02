using CesiZen.Domain.Datamodel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MongoDB.EntityFrameworkCore.Extensions;

namespace CesiZen.Infrastructure.Configuration;

public class SessionConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.ToCollection("sessions");

        builder.HasKey(u => u.Id);

        builder.HasIndex(u => u.Id);

        builder.HasIndex(x => x.SessionId)
            .IsUnique();
    }
}
