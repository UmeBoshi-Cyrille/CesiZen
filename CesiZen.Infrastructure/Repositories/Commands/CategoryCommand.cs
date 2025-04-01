using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using CesiZen.Domain.Mapper;
using CesiZen.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace CesiZen.Infrastructure.Repositories;

public class CategoryCommand : AbstractRepository, ICategoryCommand
{
    public CategoryCommand(CesizenDbContext context) : base(context)
    {
    }

    public async Task<IResult<CategoryDto>> Insert(Category entity)
    {
        try
        {
            context.Categories.Add(entity);
            await context.SaveChangesAsync();

            var result = entity.Map();

            return Result<CategoryDto>.Success(result, CategoryInfos.LogInsertionSucceeded(entity.Name));
        }
        catch (DbUpdateException ex)
        {
            return Result<CategoryDto>.Failure(CategoryErrors.LogInsertionFailed(entity.Name), entity.Name, ex.Message);
        }
        catch (Exception ex)
        {
            return Result<CategoryDto>.Failure(CategoryErrors.LogInsertionFailed(entity.Name), entity.Name, ex.Message);
        }
    }

    public async Task<IResult> Update(Category entity)
    {
        context.Entry(entity).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();

            return Result.Success(CategoryInfos.LogUpdateSucceeded(nameof(entity.Id)));
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(CategoryErrors.LogUpdateFailed(nameof(entity.Id)), nameof(entity.Id), ex.Message);
        }
        catch (Exception ex)
        {
            return Result.Failure(CategoryErrors.LogUpdateFailed(nameof(entity.Id)), nameof(entity.Id), ex.Message);
        }
    }

    public async Task<IResult> Delete(int id)
    {
        try
        {
            var Article = await context.Categories.FindAsync(id);
            if (Article != null)
            {
                context.Categories.Remove(Article);
                await context.SaveChangesAsync();

                return Result.Success(CategoryInfos.LogDeleteCompleted(nameof(id)));
            }

            return Result.Failure(CategoryErrors.LogDeletionFailed(nameof(id)));
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(CategoryErrors.LogDeletionFailed(nameof(id)), nameof(id), ex.Message);
        }
        catch (Exception ex)
        {
            return Result.Failure(CategoryErrors.LogDeletionFailed(nameof(id)), nameof(id), ex.Message);
        }
    }
}
