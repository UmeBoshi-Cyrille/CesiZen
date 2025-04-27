using CesiZen.Domain.Datamodel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CesiZen.Infrastructure.Configuration;

public class ResetPasswordConfiguration : IEntityTypeConfiguration<ResetPassword>
{
    public void Configure(EntityTypeBuilder<ResetPassword> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.Id)
            .IsUnique();

        builder.HasIndex(x => x.ResetToken);
    }
}