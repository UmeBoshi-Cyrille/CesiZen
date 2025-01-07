using CesiZen.Domain.Interface;
using CesiZen.Infrastructure.DatabaseContext;

namespace CesiZen.Infrastructure.Repository;

public class LoginQuery : AbstractRepository, ILoginQuery
{
    public LoginQuery(MongoDbContext context) : base(context)
    {
    }
}