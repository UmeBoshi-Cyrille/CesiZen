using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Interfaces;
using Serilog;

namespace CesiZen.Application.Services;

public class LoginCommandService : AService, ILoginCommandService
{
    private readonly ILoginCommand command;

    public LoginCommandService(ILoginCommand command, ILogger logger) : base(logger)
    {
        this.command = command;
    }

    public async Task<IResult> UpdateEmail(int userId, string email)
    {
        var result = await command.UpdateEmail(userId, email);

        if (result.IsSuccess)
        {
            logger.Information(result.Info.Message);
            return Result.Success(LoginInfos.ClientUpdateSucceeded);
        }

        logger.Error(result.Error.Message);
        return Result.Failure(LoginErrors.ClientUpdateFailed);
    }
}
