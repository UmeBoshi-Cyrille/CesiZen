using CesiZen.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace CesiZen.Test.Utils;

public static class Tools
{
    internal static DbContextOptions<CesizenDbContext> SetContext()
    {
        return new DbContextOptionsBuilder<CesizenDbContext>()
            .UseNpgsql("mongodb://localhost:27017")
            .Options;
    }
}
