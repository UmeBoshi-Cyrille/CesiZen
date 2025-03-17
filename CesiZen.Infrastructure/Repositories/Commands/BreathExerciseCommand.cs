using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.Interfaces;
using CesiZen.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace CesiZen.Infrastructure.Repositories;

public class BreathExerciseCommand : AbstractRepository, IBreathExerciseCommand
{
    public BreathExerciseCommand(MongoDbContext context) : base(context)
    {
    }

    public async Task<IResult> Insert(BreathExercise entity)
    {
        try
        {
            context.BreathExercises.Add(entity);
            await context.SaveChangesAsync();

            return Result.Success();
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(ArticleErrors.OperationFailed("Insert", ex.Message));
        }
        catch (Exception ex)
        {
            return Result.Failure(ArticleErrors.OperationFailed("Delete", ex.Message));
        }
    }

    public async Task<IResult> Update(BreathExercise entity)
    {
        try
        {
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(Error.NullValue(""));
        }
    }

    public async Task<IResult> Delete(string id)
    {
        try
        {
            var result = await context.BreathExercises.FindAsync(id);
            if (result != null)
            {
                context.BreathExercises.Remove(result);
                await context.SaveChangesAsync();

                return Result.Success();
            }

            return Result.Failure(Error.NullValue(""));
        }
        catch (Exception ex)
        {
            return Result.Failure(Error.NullValue(""));
        }
    }
}
