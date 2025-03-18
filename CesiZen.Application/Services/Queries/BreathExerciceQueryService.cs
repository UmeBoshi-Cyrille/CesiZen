using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using CesiZen.Domain.Mapper;
using Serilog;

namespace CesiZen.Application.Services;

public class BreathExerciseQueryService : AService, IBreathExerciseQueryService
{
    private readonly IBreathExerciseQuery query;

    public BreathExerciseQueryService(ILogger logger, IBreathExerciseQuery query) : base(logger)
    {
        this.query = query;
    }

    public async Task<IResult<List<BreathExerciseDto>>> GetAllByIdAsync(string userId)
    {
        var result = await query.GetAllByIdAsync(userId);

        if (result.IsFailure)
        {
            logger.Error(result.Error.Message);
            return Result<List<BreathExerciseDto>>.Failure(BreathExerciseErrors.ClientMultipleNotFound);
        }

        var dto = result.Value.Map();

        return Result<List<BreathExerciseDto>>.Success(dto);
    }

    public async Task<IResult<BreathExerciseDto>> GetByIdAsync(string id)
    {
        var result = await query.GetByIdAsync(id);

        if (result.IsFailure)
        {
            logger.Error(result.Error.Message);
            return Result<BreathExerciseDto>.Failure(BreathExerciseErrors.ClientNotFound);
        }

        var dto = result.Value.Map();

        return Result<BreathExerciseDto>.Success(dto);
    }
}
