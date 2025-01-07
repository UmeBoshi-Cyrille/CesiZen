using CesiZen.Domain.Interface;
using CesiZen.Infrastructure.DatabaseContext;

namespace CesiZen.Infrastructure.Repository;

public class LoginCommand : AbstractRepository, ILoginCommand
{
    public LoginCommand(MongoDbContext context) : base(context)
    {
    }
}
