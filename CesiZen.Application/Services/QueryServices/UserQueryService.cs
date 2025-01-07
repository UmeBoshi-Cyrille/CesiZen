using CesiZen.Domain.Datamodel;
using CesiZen.Domain.Interface;
using Serilog;

namespace CesiZen.Application.Services;

public class UserQueryService : IUserQuery
{
    private readonly IUserQuery query;
    private readonly ILogger logger;

    public UserQueryService(
        IUserQuery query,
        ILogger logger)
    {
        this.query = query;
        this.logger = logger;
    }

    public IEnumerable<User> GetAll()
    {
        throw new NotImplementedException();
    }

    public User GetOne(int id)
    {
        throw new NotImplementedException();
    }
}
