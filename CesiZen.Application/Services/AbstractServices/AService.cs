using Serilog;

namespace CesiZen.Application.Services;

public abstract class AService
{
    protected readonly ILogger logger;

    public AService(ILogger logger)
    {
        this.logger = logger;
    }
}
