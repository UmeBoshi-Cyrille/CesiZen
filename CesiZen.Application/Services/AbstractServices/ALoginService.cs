using CesiZen.Domain.Interfaces;
using Serilog;

namespace CesiZen.Application.Services;

public class ALoginService : AService
{
    protected readonly IUserCommand userCommand;
    protected readonly ILoginQuery loginQuery;
    protected readonly IPasswordService passwordService;
    protected readonly ITokenProvider tokenProvider;

    public ALoginService(
        ILogger logger,
        IUserCommand userCommand,
        IPasswordService passwordService,
        ILoginQuery loginQuery,
        ITokenProvider tokenProvider
        ) : base(logger)
    {
        this.loginQuery = loginQuery;
        this.userCommand = userCommand;
        this.passwordService = passwordService;
        this.tokenProvider = tokenProvider;
    }
}
