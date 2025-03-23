using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using CesiZen.Domain.Mapper;
using Serilog;

namespace CesiZen.Application.Services;

public class CategoryCommandService : AService, ICategoryCommandService
{
    private readonly ICategoryCommand command;
    public CategoryCommandService(ILogger logger, ICategoryCommand command) : base(logger)
    {
        this.command = command;
    }

    public async Task<IResult> Insert(CategoryDto dto)
    {
        var entity = dto.Map();
        var result = await command.Insert(entity);

        if (result.IsFailure)
        {
            logger.Error(result.Error.Message);
            return Result.Failure(CategoryErrors.ClientInsertionFailed);
        }

        logger.Information(result.Info.Message);
        return Result.Success(CategoryInfos.ClientInsertionSucceeded);
    }

    public async Task<IResult> Update(CategoryDto dto)
    {
        var entity = dto.Map();
        var result = await command.Update(entity);

        if (result.IsFailure)
        {
            logger.Error(result.Error.Message);
            return Result.Failure(CategoryErrors.ClientUpdateFailed);
        }

        logger.Information(result.Info.Message);
        return Result.Success(CategoryInfos.ClientUpdateSucceeded);
    }

    public async Task<IResult> Delete(int id)
    {
        var result = await command.Delete(id);

        if (result.IsFailure)
        {
            logger.Error(result.Error.Message);
            return Result.Failure(CategoryErrors.ClientDeletionFailed);
        }

        logger.Information(result.Info.Message);
        return Result.Success(CategoryInfos.ClientDeleteCompleted);
    }
}
