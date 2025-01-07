using CesiZen.Domain.Interface;
using Serilog;

namespace CesiZen.Application.Services;

public class LoginCommandService : ILoginCommandService
{
    private readonly ILoginCommand command;
    private readonly ILogger logger;

    public LoginCommandService(
        ILoginCommand command,
        ILogger logger)
    {
        this.command = command;
        this.logger = logger;
    }
}
