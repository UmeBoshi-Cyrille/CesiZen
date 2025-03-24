using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using CesiZen.Domain.Mapper;
using Serilog;

namespace CesiZen.Application.Services;

public class BreathExerciseCommandService : AService, IBreathExerciseCommandService
{
    private readonly IBreathExerciseCommand command;

    public BreathExerciseCommandService(
        ILogger logger,
        IBreathExerciseCommand command) : base(logger)
    {
        this.command = command;
    }

    public async Task<IResult> Insert(NewBreathExerciseDto dto)
    {
        var entity = dto.Map();
        var result = await command.Insert(entity);

        if (result.IsFailure)
        {
            logger.Error(result.Error.Message);
            return Result.Failure(BreathExerciseErrors.ClientInsertionFailed);
        }

        return Result.Success(BreathExerciseInfos.ClientInsertionSucceeded);
    }

    public async Task<IResult> Update(BreathExerciseDto dto)
    {
        var entity = dto.Map();
        var result = await command.Update(entity);

        if (result.IsFailure)
        {
            logger.Error(result.Error.Message);
            return Result.Failure(BreathExerciseErrors.ClientUpdateFailed);
        }

        return Result.Success(BreathExerciseInfos.ClientUpdateSucceeded);
    }

    public async Task<IResult> Delete(int id)
    {
        var result = await command.Delete(id);

        if (result.IsFailure)
        {
            logger.Error(result.Error.Message);
            return Result.Failure(BreathExerciseErrors.ClientDeletionFailed);
        }

        return Result.Success(BreathExerciseInfos.ClientDeleteCompleted);
    }
}
