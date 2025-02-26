using CesiZen.Domain.Interface;
using CesiZen.Domain.Interfaces;
using Serilog;

namespace CesiZen.Application.Services;

public class ALoginService : AService
{
    protected readonly IUserCommand userCommand;
    protected readonly ILoginQuery loginQuery;
    protected readonly IPasswordService passwordService;
    protected readonly IEmailService emailService;

    public ALoginService(
        ILogger logger,
        IUserCommand userCommand,
        IPasswordService passwordService,
        ILoginQuery loginQuery,
        IEmailService emailService
        ) : base(logger)
    {
        this.loginQuery = loginQuery;
        this.userCommand = userCommand;
        this.passwordService = passwordService;
        this.emailService = emailService;
    }
}
