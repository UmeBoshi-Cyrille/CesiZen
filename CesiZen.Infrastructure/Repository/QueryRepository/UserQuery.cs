using CesiZen.Domain.Datamodel;
using CesiZen.Domain.Interface;
using CesiZen.Infrastructure.DatabaseContext;

namespace CesiZen.Infrastructure.Repository;

public class UserQuery : AbstractRepository, IUserQuery
{
    public UserQuery(MongoDbContext context) : base(context)
    {
    }

    IEnumerable<User> IQuery<User>.GetAll()
    {
        throw new NotImplementedException();
    }

    User IQuery<User>.GetOne(int id)
    {
        throw new NotImplementedException();
    }
}
