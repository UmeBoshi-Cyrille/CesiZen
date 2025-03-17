using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.Interfaces;
using CesiZen.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace CesiZen.Infrastructure.Repositories;

public class ArticleCommand : AbstractRepository, IArticleCommand
{
    public ArticleCommand(MongoDbContext context) : base(context)
    {
    }

    public async Task<IResult> Insert(Article article)
    {
        try
        {
            context.Articles.Add(article);
            await context.SaveChangesAsync();

            return Result.Success();
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(ArticleErrors.OperationFailed("Insert", ex.Message));
        }
        catch (Exception ex)
        {
            return Result.Failure(ArticleErrors.OperationFailed("Insert", ex.Message));
        }
    }

    public async Task<IResult> Update(Article article)
    {
        context.Entry(article).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();

            return Result.Success();
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(ArticleErrors.OperationFailed("Update", ex.Message));
        }
        catch (Exception ex)
        {
            return Result.Failure(ArticleErrors.OperationFailed("Update", ex.Message));
        }
    }

    public async Task<IResult> UpdateTitleAsync(Article article)
    {
        try
        {
            context.Attach(article);
            context.Entry(article).Property(p => p.Title).IsModified = true;
            context.Entry(article).Property(p => p.UpdatedAt).IsModified = true;

            await context.SaveChangesAsync();

            return Result.Success();
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(ArticleErrors.OperationFailed("Update", ex.Message));
        }
        catch (Exception ex)
        {
            return Result.Failure(ArticleErrors.OperationFailed("Update", ex.Message));
        }
    }

    public async Task<IResult> UpdateDescriptionAsync(Article article)
    {
        try
        {
            context.Attach(article);
            context.Entry(article).Property(p => p.Description).IsModified = true;
            context.Entry(article).Property(p => p.UpdatedAt).IsModified = true;

            await context.SaveChangesAsync();

            return Result.Success();
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(ArticleErrors.OperationFailed("UpdateDescription", ex.Message));
        }
        catch (Exception ex)
        {
            return Result.Failure(ArticleErrors.OperationFailed("UpdateDescription", ex.Message));
        }
    }

    public async Task<IResult> UpdateContentAsync(Article article)
    {
        try
        {
            context.Attach(article);
            context.Entry(article).Property(p => p.Content).IsModified = true;
            context.Entry(article).Property(p => p.UpdatedAt).IsModified = true;

            await context.SaveChangesAsync();

            return Result.Success();
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(ArticleErrors.OperationFailed("Delete", ex.Message));
        }
        catch (Exception ex)
        {
            return Result.Failure(ArticleErrors.OperationFailed("Delete", ex.Message));
        }
    }

    public async Task<IResult> Delete(string id)
    {
        try
        {
            var article = await context.Articles.FindAsync(id);
            if (article != null)
            {
                context.Articles.Remove(article);
                await context.SaveChangesAsync();

                return Result.Success();
            }

            return Result.Failure(ArticleErrors.DeletionFailedToLog);
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(ArticleErrors.OperationFailed("Delete", ex.Message));
        }
        catch (Exception ex)
        {
            return Result.Failure(ArticleErrors.OperationFailed("Delete", ex.Message));
        }
    }
}
