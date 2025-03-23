using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.Interfaces;
using CesiZen.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace CesiZen.Infrastructure.Repositories;

public class ArticleCommand : AbstractRepository, IArticleCommand
{
    public ArticleCommand(CesizenDbContext context) : base(context)
    {
    }

    public async Task<IResult> Insert(Article article)
    {
        try
        {
            context.Articles.Add(article);
            await context.SaveChangesAsync();

            return Result.Success(ArticleInfos.LogInsertionSucceeded(article.Author, article.Title));
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(ArticleErrors.LogInsertionFailed(article.Title), article.Author, ex.Message);
        }
        catch (Exception ex)
        {
            return Result.Failure(ArticleErrors.LogInsertionFailed(article.Title), article.Author, ex.Message);
        }
    }

    public async Task<IResult> Update(Article article)
    {
        context.Entry(article).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();

            return Result.Success(ArticleInfos.LogUpdateSucceeded(article.Id));
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(ArticleErrors.LogUpdateFailed(article.Id), article.Id, ex.Message);
        }
        catch (Exception ex)
        {
            return Result.Failure(ArticleErrors.LogUpdateFailed(article.Id), article.Id, ex.Message);
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

            return Result.Success(ArticleInfos.LogUpdateProperty("title"));
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(ArticleErrors.LogUpdatePropertyFailed("title", article.Id), article.Id, ex.Message);
        }
        catch (Exception ex)
        {
            return Result.Failure(ArticleErrors.LogUpdatePropertyFailed("title", article.Id), article.Id, ex.Message);
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

            return Result.Success(ArticleInfos.LogUpdateProperty("description"));
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(ArticleErrors.LogUpdatePropertyFailed("description", article.Id), article.Id, ex.Message);
        }
        catch (Exception ex)
        {
            return Result.Failure(ArticleErrors.LogUpdatePropertyFailed("description", article.Id), article.Id, ex.Message);
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

            return Result.Success(ArticleInfos.LogUpdateProperty("content"));
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(ArticleErrors.LogUpdatePropertyFailed("content", article.Id), article.Id, ex.Message);
        }
        catch (Exception ex)
        {
            return Result.Failure(ArticleErrors.LogUpdatePropertyFailed("content", article.Id), article.Id, ex.Message);
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

                return Result.Success(ArticleInfos.LogDeleteCompleted(id));
            }

            return Result.Failure(ArticleErrors.LogDeletionFailed(id));
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(ArticleErrors.LogDeletionFailed(id), id, ex.Message);
        }
        catch (Exception ex)
        {
            return Result.Failure(ArticleErrors.LogDeletionFailed(id), id, ex.Message);
        }
    }
}
