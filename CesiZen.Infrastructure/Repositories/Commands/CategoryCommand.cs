using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.Interfaces;
using CesiZen.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace CesiZen.Infrastructure.Repositories;

public class CategoryCommand : AbstractRepository, ICategoryCommand
{
    public CategoryCommand(MongoDbContext context) : base(context)
    {
    }

    public async Task<IResult> Insert(Category entity)
    {
        try
        {
            context.Categories.Add(entity);
            await context.SaveChangesAsync();

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(Error.NullValue(""));
        }
    }

    public async Task<IResult> Update(Category entity)
    {
        context.Entry(entity).State = EntityState.Modified;

        try
        {
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
            var Article = await context.Categories.FindAsync(id);
            if (Article != null)
            {
                context.Categories.Remove(Article);
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
