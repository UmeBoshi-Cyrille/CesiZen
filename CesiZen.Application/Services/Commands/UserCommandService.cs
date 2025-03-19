using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using CesiZen.Domain.Mapper;
using Serilog;

namespace CesiZen.Application.Services;

public class UserCommandService : AService, IUserCommandService
{
    private readonly IUserCommand command;

    public UserCommandService(IUserCommand command, ILogger logger) : base(logger)
    {
        this.command = command;
    }

    public async Task<IResult> Update(UserDto dto)
    {
        var user = dto.Map();
        var result = await command.Update(user);

        if (result.IsSuccess)
        {
            logger.Information(result.Info.Message);
            return Result.Success(UserInfos.ClientUpdateSucceeded);
        }

        logger.Error(result.Error.Message);
        return Result.Failure(UserErrors.ClientUpdateFailed);
    }

    public async Task<IResult> Delete(string id)
    {
        var result = await command.Delete(id);

        if (result.IsSuccess)
        {
            logger.Information(result.Info.Message);
            return Result.Success(UserInfos.ClientDeleteCompleted);
        }

        logger.Error(result.Error.Message);
        return Result.Failure(UserErrors.ClientDeletionFailed);
    }

    public async Task<IResult> UpdateUserName(string id, string userName)
    {
        var result = await command.UpdateUserName(id, userName);

        if (result.IsSuccess)
        {
            logger.Information(result.Info.Message);
            return Result.Success(UserInfos.ClientUpdateSucceeded);
        }

        logger.Error(result.Error.Message);
        return Result.Failure(UserErrors.ClientUpdateFailed);
    }

    public async Task<IResult> ActivationAsync(AccountActivationDto dto)
    {
        var user = dto.Map();
        var result = await command.ActivationAsync(user);

        if (result.IsSuccess)
        {
            Info info = new();
            if (dto.IsActive)
                info = UserInfos.ClientAccountEnabled;
            else
                info = UserInfos.ClientAccountDisabled;

            logger.Information(result.Info.Message);
            return Result.Success(info);
        }

        logger.Error(result.Error.Message);
        return Result.Failure(UserErrors.ClientUpdateFailed);
    }
}
