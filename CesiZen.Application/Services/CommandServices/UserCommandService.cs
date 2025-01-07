using CesiZen.Domain.Datamodel;
using CesiZen.Domain.Interface;
using Serilog;

namespace CesiZen.Application.Services;

public class UserCommandService : IUserCommandService
{
    private readonly IUserCommand command;
    private readonly ILogger logger;

    public UserCommandService(
        IUserCommand command,
        ILogger logger)
    {
        this.command = command;
        this.logger = logger;
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public void Insert(User entity)
    {
        throw new NotImplementedException();
    }

    public void Update(User entity)
    {
        throw new NotImplementedException();
    }
}
