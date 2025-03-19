using CesiZen.Domain.Datamodel;

using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CesiZen.Infrastructure.DatabaseContext;

public class MongoDbContext : DbContext
{
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Login> Logins { get; set; }
    public virtual DbSet<Session> Sessions { get; set; }
    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
    public virtual DbSet<Article> Articles { get; set; }
    public virtual DbSet<BreathExercise> BreathExercises { get; set; }


    public MongoDbContext(DbContextOptions options)
        : base(options)
    {
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        this.Database.AutoTransactionBehavior = AutoTransactionBehavior.Never;
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
