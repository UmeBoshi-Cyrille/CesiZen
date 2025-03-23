using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.Interfaces;
using CesiZen.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace CesiZen.Infrastructure.Repositories;

public class BreathExerciseCommand : AbstractRepository, IBreathExerciseCommand
{
    public BreathExerciseCommand(CesizenDbContext context) : base(context)
    {
    }

    public async Task<IResult> Insert(BreathExercise entity)
    {
        try
        {
            context.BreathExercises.Add(entity);
            await context.SaveChangesAsync();

            return Result.Success(BreathExerciseInfos.LogInsertionSucceeded(entity.Title));
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(BreathExerciseErrors.LogInsertionFailed(entity.Title), nameof(entity.UserId), ex.Message);
        }
        catch (Exception ex)
        {
            return Result.Failure(BreathExerciseErrors.LogInsertionFailed(entity.Title), nameof(entity.UserId), ex.Message);
        }
    }

    public async Task<IResult> Update(BreathExercise entity)
    {
        try
        {
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();

            return Result.Success(BreathExerciseInfos.LogUpdateSucceeded(nameof(entity.Id)));
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(BreathExerciseErrors.LogUpdateFailed(nameof(entity.Id)), nameof(entity.Id), ex.Message);
        }
        catch (Exception ex)
        {
            return Result.Failure(BreathExerciseErrors.LogUpdateFailed(nameof(entity.Id)), nameof(entity.Id), ex.Message);
        }
    }

    public async Task<IResult> Delete(int id)
    {
        try
        {
            var result = await context.BreathExercises.FindAsync(id);
            if (result != null)
            {
                context.BreathExercises.Remove(result);
                await context.SaveChangesAsync();

                return Result.Success(BreathExerciseInfos.LogDeleteCompleted(nameof(id)));
            }

            return Result.Failure(BreathExerciseErrors.LogDeletionFailed(nameof(id)));
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(BreathExerciseErrors.LogDeletionFailed(nameof(id)), nameof(id), ex.Message);
        }
        catch (Exception ex)
        {
            return Result.Failure(BreathExerciseErrors.LogDeletionFailed(nameof(id)), nameof(id), ex.Message);
        }
    }
}
