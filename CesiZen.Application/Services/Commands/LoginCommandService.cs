using CesiZen.Domain.Interfaces;

namespace CesiZen.Application.Services;

public class LoginCommandService : ILoginCommandService
{
    private readonly ILoginCommand command;

    public LoginCommandService(ILoginCommand command)
    {
        this.command = command;
    }

    public Task UpdateEmail(int userId, string email)
    {
        throw new NotImplementedException();
    }

    public Task UpdatePassword(int userId, string password)
    {
        throw new NotImplementedException();
    }
}
