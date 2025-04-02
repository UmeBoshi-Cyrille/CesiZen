using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using CesiZen.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace CesiZen.Infrastructure.Repositories;

public class ArticleQuery : AbstractRepository, IArticleQuery
{
    public ArticleQuery(CesizenDbContext context) : base(context)
    {
    }

    public async Task<IResult<PagedResultDto<Article>>> GetAllAsync(int pageNumber, int pageSize)
    {
        var articles = await context.Articles
               .AsNoTracking()
               .Include(c => c.Categories)
               .Skip((pageNumber - 1) * pageSize)
               .Take(pageSize)
               .OrderBy(x => x.Title)
               .ToListAsync();

        if (!articles.Any())
        {
            return Result<PagedResultDto<Article>>.Failure(ArticleErrors.LogMultipleNotFound);
        }

        var result = new PagedResultDto<Article>
        {
            Data = articles,
            TotalCount = articles.Count,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        return Result<PagedResultDto<Article>>.Success(result);
    }

    public async Task<IResult<PagedResultDto<Article>>> SearchArticles(PageParametersDto parameters, string searchTerm = "")
    {
        try
        {
            var query = context.Articles.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(a => a.Title.Contains(searchTerm) || a.Content.Contains(searchTerm))
                    .Include(c => c.Categories);
            }

            var totalCount = await query.CountAsync();
            var Articles = await query
                .OrderByDescending(a => a.CreatedAt)
                .Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            var result = new PagedResultDto<Article>
            {
                Data = Articles,
                TotalCount = totalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };

            return Result<PagedResultDto<Article>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<PagedResultDto<Article>>.Failure(ArticleErrors.LogMultipleNotFound, ex.Message);
        }
    }

    public async Task<IResult<Article>> GetByIdAsync(int id)
    {
        try
        {
            var result = await context.Articles
                    .AsNoTracking()
                    .Include(x => x.Images)
                    .FirstOrDefaultAsync(x => x.Id == id);

            return Result<Article>.Success(result!);
        }
        catch (Exception ex)
        {
            return Result<Article>.Failure(ArticleErrors.LogNotFound(nameof(id)), nameof(id), ex.Message);
        }
    }
}
