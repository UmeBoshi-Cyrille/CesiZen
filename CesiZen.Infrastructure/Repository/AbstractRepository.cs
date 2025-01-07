using CesiZen.Infrastructure.DatabaseContext;

namespace CesiZen.Infrastructure.Repository;

public class AbstractRepository
{
    private readonly MongoDbContext context;

    public AbstractRepository(MongoDbContext context)
    {
        this.context = context;
    }
}
