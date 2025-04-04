using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using CesiZen.Domain.Mapper;
using CesiZen.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace CesiZen.Infrastructure.Repositories;

public class BreathExerciseQuery : AbstractRepository, IBreathExerciseQuery
{
    public BreathExerciseQuery(CesizenDbContext context) : base(context)
    {
    }

    public async Task<IResult<List<BreathExerciseMinimumDto>>> GetAllByIdAsync(int userId)
    {
        try
        {
            var result = await context.BreathExercises
                                .Where(p => p.UserId == userId)
                                .OrderBy(p => p.EditedAt)
                                .Select(x => x.MapMinimumDto())
                                .ToListAsync();

            if (result is null)
            {
                return Result<List<BreathExerciseMinimumDto>>.Failure(BreathExerciseErrors.LogMultipleNotFound);
            }

            return Result<List<BreathExerciseMinimumDto>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<List<BreathExerciseMinimumDto>>.Failure(BreathExerciseErrors.LogMultipleNotFound, ex.Message);
        }
    }

    public async Task<IResult<BreathExerciseDto>> GetByIdAsync(int id)
    {
        try
        {
            var result = await context.BreathExercises
                .Where(x => x.Id == id)
                .Select(x => x.Map())
                .FirstOrDefaultAsync();

            return Result<BreathExerciseDto>.Success(result!);
        }
        catch (Exception ex)
        {
            return Result<BreathExerciseDto>.Failure(BreathExerciseErrors.LogNotFound(nameof(id)), nameof(id), ex.Message);
        }
    }
}
