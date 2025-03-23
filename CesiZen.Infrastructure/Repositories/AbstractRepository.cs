using CesiZen.Infrastructure.DatabaseContext;

namespace CesiZen.Infrastructure.Repositories;

public class AbstractRepository
{
    protected readonly CesizenDbContext context;

    public AbstractRepository(CesizenDbContext context)
    {
        this.context = context;
    }
}
