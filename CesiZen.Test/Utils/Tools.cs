using CesiZen.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace CesiZen.Test.Utils;

public static class Tools
{
    internal static DbContextOptions<MongoDbContext> SetContext()
    {
        return new DbContextOptionsBuilder<MongoDbContext>()
            .UseMongoDB("mongodb://localhost:27017", "TestDB")
            .Options;
    }
}
