using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.Interfaces;
using CesiZen.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace CesiZen.Infrastructure.Repositories;

public class CategoryCommand : AbstractRepository, ICategoryCommand
{
    public CategoryCommand(CesizenDbContext context) : base(context)
    {
    }

    public async Task<IResult> Insert(Category entity)
    {
        try
        {
            context.Categories.Add(entity);
            await context.SaveChangesAsync();

            return Result.Success(CategoryInfos.LogInsertionSucceeded(entity.Name));
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(CategoryErrors.LogInsertionFailed(entity.Name), entity.Name, ex.Message);
        }
        catch (Exception ex)
        {
            return Result.Failure(CategoryErrors.LogInsertionFailed(entity.Name), entity.Name, ex.Message);
        }
    }

    public async Task<IResult> Update(Category entity)
    {
        context.Entry(entity).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();

            return Result.Success(CategoryInfos.LogUpdateSucceeded(entity.Id));
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(CategoryErrors.LogUpdateFailed(entity.Id), entity.Id, ex.Message);
        }
        catch (Exception ex)
        {
            return Result.Failure(CategoryErrors.LogUpdateFailed(entity.Id), entity.Id, ex.Message);
        }
    }

    public async Task<IResult> Delete(string id)
    {
        try
        {
            var Article = await context.Categories.FindAsync(id);
            if (Article != null)
            {
                context.Categories.Remove(Article);
                await context.SaveChangesAsync();

                return Result.Success(CategoryInfos.LogDeleteCompleted(id));
            }

            return Result.Failure(CategoryErrors.LogDeletionFailed(id));
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(CategoryErrors.LogDeletionFailed(id), id, ex.Message);
        }
        catch (Exception ex)
        {
            return Result.Failure(CategoryErrors.LogDeletionFailed(id), id, ex.Message);
        }
    }
}
