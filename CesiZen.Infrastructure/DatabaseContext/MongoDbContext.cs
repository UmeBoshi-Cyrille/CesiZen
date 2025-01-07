using CesiZen.Domain.Datamodel;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CesiZen.Infrastructure.DatabaseContext;

public class MongoDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public MongoDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
