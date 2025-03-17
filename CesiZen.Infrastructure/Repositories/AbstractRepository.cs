using CesiZen.Infrastructure.DatabaseContext;

namespace CesiZen.Infrastructure.Repositories;

public class AbstractRepository
{
    protected readonly MongoDbContext context;

    public AbstractRepository(MongoDbContext context)
    {
        this.context = context;
    }
}
