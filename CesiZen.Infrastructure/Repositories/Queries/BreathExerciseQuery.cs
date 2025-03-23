using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.Interfaces;
using CesiZen.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace CesiZen.Infrastructure.Repositories;

public class BreathExerciseQuery : AbstractRepository, IBreathExerciseQuery
{
    public BreathExerciseQuery(CesizenDbContext context) : base(context)
    {
    }

    public async Task<IResult<List<BreathExercise>>> GetAllByIdAsync(string userId)
    {
        try
        {
            var result = await context.BreathExercises
                                .Where(p => p.UserId == userId)
                                .ToListAsync();

            if (result is null)
            {
                return Result<List<BreathExercise>>.Failure(BreathExerciseErrors.LogMultipleNotFound);
            }

            return Result<List<BreathExercise>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<List<BreathExercise>>.Failure(BreathExerciseErrors.LogMultipleNotFound, ex.Message);
        }
    }

    public async Task<IResult<BreathExercise>> GetByIdAsync(string id)
    {
        try
        {
            var result = await context.BreathExercises.FindAsync(id);

            return Result<BreathExercise>.Success(result!);
        }
        catch (Exception ex)
        {
            return Result<BreathExercise>.Failure(BreathExerciseErrors.LogNotFound(id), id, ex.Message);
        }
    }
}
