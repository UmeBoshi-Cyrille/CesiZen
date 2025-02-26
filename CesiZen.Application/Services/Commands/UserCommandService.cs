using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.Interface;
using Serilog;

namespace CesiZen.Application.Services;

public class UserCommandService : AService, IUserCommandService
{
    private readonly IUserCommand command;

    public UserCommandService(IUserCommand command, ILogger logger) : base(logger)
    {
        this.command = command;
    }

    public async Task<IResult> Insert(User entity)
    {
        var result = await command.Insert(entity);

        if (result.IsSuccess)
        {
            logger.Information(result.Info.Message);
            return Result.Success(
                        Info.Success(string.Format(
                            Message.GetResource("InfoMessages", "CLIENT_CREATION_SUCCESS"), "User")));
        }

        logger.Error(result.Error.Message);
        return Result.Failure(
                  Error.OperationFailed(string.Format(
                      Message.GetResource("ErrorMessages", "CLIENT_INSERTION_FAILED"), "User")));
    }

    public async Task<IResult> Update(User entity)
    {
        var result = await command.Update(entity);

        if (result.IsSuccess)
        {
            logger.Information(result.Info.Message);
            return Result.Success(
                        Info.Success(string.Format(
                            Message.GetResource("InfoMessages", "CLIENT_UPDATE_SUCCESS"), "User")));
        }

        logger.Error(result.Error.Message);
        return Result.Failure(
                    Error.OperationFailed(string.Format(
                      Message.GetResource("ErrorMessages", "CLIENT_UPDATE_FAILED"), "User")));
    }

    public async Task<IResult> Delete(int id)
    {
        var result = await command.Delete(id);

        if (result.IsSuccess)
        {
            logger.Information(result.Info.Message);
            return Result.Success(
                        Info.Success(string.Format(
                            Message.GetResource("InfoMessages", "CLIENT_DELETE_SUCCESS"), "User")));
        }

        logger.Error(result.Error.Message);
        return Result.Failure(Error.OperationFailed(string.Format(
                      Message.GetResource("ErrorMessages", "CLIENT_DELETION_FAILED"), "User")));
    }

    public async Task<IResult> UpdatePostalCode(int id, uint postalCode)
    {
        var result = await command.UpdatePostalCode(id, postalCode);

        if (result.IsSuccess)
        {
            logger.Information(result.Info.Message);
            return Result.Success(
                        Info.Success(string.Format(
                            Message.GetResource("InfoMessages", "CLIENT_UPDATE_SUCCESS"), "User")));
        }

        logger.Error(result.Error.Message);
        return Result.Failure(
                    Error.OperationFailed(string.Format(
                      Message.GetResource("ErrorMessages", "CLIENT_UPDATE_PROPERTY_FAILED"), "PostalCode")));
    }

    public async Task<IResult> UpdateUserName(int id, string userName)
    {
        var result = await command.UpdateUserName(id, userName);

        if (result.IsSuccess)
        {
            logger.Information(result.Info.Message);
            return Result.Success(
                        Info.Success(string.Format(
                            Message.GetResource("InfoMessages", "CLIENT_UPDATE_SUCCESS"), "User")));
        }

        logger.Error(result.Error.Message);
        return Result.Failure(
                   Error.OperationFailed(string.Format(
                      Message.GetResource("ErrorMessages", "CLIENT_UPDATE_PROPERTY_FAILED"), "UserName")));
    }
}
