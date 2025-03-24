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
        var fetchArticle = await context.Articles
            .Include(x => x.Images)
            .FirstOrDefaultAsync(x => x.Id == article.Id);

        if (fetchArticle == null)
        {
            return Result.Failure(ArticleErrors.LogNotFound(nameof(article.Id)));
        }

        try
        {
            fetchArticle!.Title = article.Title;
            fetchArticle.Author = article.Author;
            fetchArticle.Description = article.Description;
            fetchArticle.Content = article.Content;
            fetchArticle.UpdatedAt = DateTime.UtcNow;

            if (article.Images!.Any())
            {
                var img = article.Images!.ToList();

                for (int i = 0; i < article.Images!.ToList().Count; i++)
                {
                    fetchArticle.Images!.ToList().Add(img[i]);
                }
            }

            context.Entry(fetchArticle).State = EntityState.Modified;

            await context.SaveChangesAsync();

            return Result.Success(ArticleInfos.LogUpdateSucceeded(nameof(article.Id)));
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(ArticleErrors.LogUpdateFailed(nameof(article.Id)), nameof(article.Id), ex.Message);
        }
        catch (Exception ex)
        {
            return Result.Failure(ArticleErrors.LogUpdateFailed(nameof(article.Id)), nameof(article.Id), ex.Message);
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
            return Result.Failure(ArticleErrors.LogUpdatePropertyFailed("title", nameof(article.Id)), nameof(article.Id), ex.Message);
        }
        catch (Exception ex)
        {
            return Result.Failure(ArticleErrors.LogUpdatePropertyFailed("title", nameof(article.Id)), nameof(article.Id), ex.Message);
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
            return Result.Failure(ArticleErrors.LogUpdatePropertyFailed("description", nameof(article.Id)), nameof(article.Id), ex.Message);
        }
        catch (Exception ex)
        {
            return Result.Failure(ArticleErrors.LogUpdatePropertyFailed("description", nameof(article.Id)), nameof(article.Id), ex.Message);
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
            return Result.Failure(ArticleErrors.LogUpdatePropertyFailed("content", nameof(article.Id)), nameof(article.Id), ex.Message);
        }
        catch (Exception ex)
        {
            return Result.Failure(ArticleErrors.LogUpdatePropertyFailed("content", nameof(article.Id)), nameof(article.Id), ex.Message);
        }
    }

    public async Task<IResult> Delete(int id)
    {
        try
        {
            var article = await context.Articles.FindAsync(id);
            if (article != null)
            {
                context.Articles.Remove(article);
                await context.SaveChangesAsync();

                return Result.Success(ArticleInfos.LogDeleteCompleted(nameof(id)));
            }

            return Result.Failure(ArticleErrors.LogDeletionFailed(nameof(id)));
        }
        catch (DbUpdateException ex)
        {
            return Result.Failure(ArticleErrors.LogDeletionFailed(nameof(id)), nameof(id), ex.Message);
        }
        catch (Exception ex)
        {
            return Result.Failure(ArticleErrors.LogDeletionFailed(nameof(id)), nameof(id), ex.Message);
        }
    }
}
