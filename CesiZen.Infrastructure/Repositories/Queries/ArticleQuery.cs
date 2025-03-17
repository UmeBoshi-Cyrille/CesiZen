using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using CesiZen.Domain.Interfaces;
using CesiZen.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace CesiZen.Infrastructure.Repositories;

public class ArticleQuery : AbstractRepository, IArticleQuery
{
    public ArticleQuery(MongoDbContext context) : base(context)
    {
    }

    public async Task<IResult<PagedResult<Article>>> GetAllAsync(int pageNumber, int pageSize)
    {
        var articles = await context.Articles
               .AsNoTracking()
               .OrderBy(x => x.Title)
               .Skip((pageNumber - 1) * pageSize)
               .Take(pageSize)
               .ToListAsync();

        if (!articles.Any())
        {
            return Result<PagedResult<Article>>.Failure(
                Error.NotFound(string.Format(
                    Message.GetResource("ErrorMessages", "LOG_GET_MULTIPLE_NOTFOUND"), "Users")));
        }

        var result = new PagedResult<Article>
        {
            Data = articles,
            TotalCount = articles.Count,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        return Result<PagedResult<Article>>.Success(result);
    }

    public async Task<IResult<PagedResult<Article>>> SearchArticles(PageParameters parameters, string searchTerm = null)
    {
        try
        {
            var query = context.Articles.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(a => a.Title.Contains(searchTerm) || a.Content.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();
            var Articles = await query
                .OrderByDescending(a => a.CreatedAt)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            var result = new PagedResult<Article>
            {
                Data = Articles,
                TotalCount = totalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };

            return Result<PagedResult<Article>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<PagedResult<Article>>.Failure(Error.NullValue(""));
        }
    }

    public Task<IResult<PagedResult<Article>>> GetArticlesAsync(PageParameters parameters, string searchTerm = null)
    {
        throw new NotImplementedException();
    }

    public async Task<IResult<Article>> GetByIdAsync(int id)
    {
        try
        {
            var result = await context.Articles.FindAsync(id);

            return Result<Article>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<Article>.Failure(Error.NullValue(""));
        }
    }
}
