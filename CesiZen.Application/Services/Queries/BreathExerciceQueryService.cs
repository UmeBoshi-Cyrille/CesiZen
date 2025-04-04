using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using Serilog;

namespace CesiZen.Application.Services;

public class BreathExerciseQueryService : AService, IBreathExerciseQueryService
{
    private readonly IBreathExerciseQuery query;

    public BreathExerciseQueryService(ILogger logger, IBreathExerciseQuery query) : base(logger)
    {
        this.query = query;
    }

    public async Task<IResult<List<BreathExerciseMinimumDto>>> GetAllByIdAsync(int userId)
    {
        var result = await query.GetAllByIdAsync(userId);

        if (result.IsFailure)
        {
            logger.Error(result.Error.Message);
            return Result<List<BreathExerciseMinimumDto>>.Failure(BreathExerciseErrors.ClientMultipleNotFound);
        }

        return Result<List<BreathExerciseMinimumDto>>.Success(result.Value);
    }

    public async Task<IResult<BreathExerciseDto>> GetByIdAsync(int id)
    {
        var result = await query.GetByIdAsync(id);

        if (result.IsFailure)
        {
            logger.Error(result.Error.Message);
            return Result<BreathExerciseDto>.Failure(BreathExerciseErrors.ClientNotFound);
        }

        return Result<BreathExerciseDto>.Success(result.Value);
    }
}
