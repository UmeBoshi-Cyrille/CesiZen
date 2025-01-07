using CesiZen.Domain.Datamodel;
using CesiZen.Domain.Interface;
using CesiZen.Infrastructure.DatabaseContext;

namespace CesiZen.Infrastructure.Repository;

internal class UserCommand : AbstractRepository, IUserCommand
{
    public UserCommand(MongoDbContext context) : base(context)
    {
    }

    void ICommand<User>.Delete(int id)
    {
        throw new NotImplementedException();
    }

    void ICommand<User>.Insert(User entity)
    {
        throw new NotImplementedException();
    }

    void ICommand<User>.Update(User entity)
    {
        throw new NotImplementedException();
    }
}
