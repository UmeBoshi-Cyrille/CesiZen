using CesiZen.Domain.Interface;
using Serilog;

namespace CesiZen.Application.Services;

public class LoginQueryService : ILoginQueryService
{
    private readonly ILoginQuery query;
    private readonly ILogger logger;

    public LoginQueryService(
        ILoginQuery query,
        ILogger logger)
    {
        this.query = query;
        this.logger = logger;
    }
}
