using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using CesiZen.Domain.Mapper;
using CesiZen.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace CesiZen.Infrastructure.Repositories;

public class ArticleQuery : AbstractRepository, IArticleQuery
{
    public ArticleQuery(CesizenDbContext context) : base(context)
    {
    }

    public async Task<IResult<PagedResultDto<ArticleMinimumDto>>> GetAllAsync(int pageNumber, int pageSize)
    {
        var articles = await context.Articles
               .AsNoTracking()
               .OrderBy(x => x.Title)
               .Skip((pageNumber - 1) * pageSize)
               .Take(pageSize)
               .Select(x => x.MapMinimumDto())
               .ToListAsync();

        if (!articles.Any())
        {
            return Result<PagedResultDto<ArticleMinimumDto>>.Failure(ArticleErrors.LogMultipleNotFound);
        }

        var result = new PagedResultDto<ArticleMinimumDto>
        {
            Data = articles,
            TotalCount = articles.Count,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        return Result<PagedResultDto<ArticleMinimumDto>>.Success(result);
    }

    public async Task<IResult<PagedResultDto<ArticleMinimumDto>>> SearchArticles(PageParametersDto parameters, string searchTerm = "")
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
                .Select(x => x.MapMinimumDto())
                .ToListAsync();

            var result = new PagedResultDto<ArticleMinimumDto>
            {
                Data = Articles,
                TotalCount = totalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };

            return Result<PagedResultDto<ArticleMinimumDto>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<PagedResultDto<ArticleMinimumDto>>.Failure(ArticleErrors.LogMultipleNotFound, ex.Message);
        }
    }

    public async Task<IResult<List<ArticleMinimumDto>>> GetLast(int amount)
    {
        try
        {
            var result = await context.Articles
                   .AsNoTracking()
                   .OrderByDescending(c => c.CreatedAt)
                   .Take(amount)
                   .Select(x => x.MapMinimumDto())
                   .ToListAsync();

            return Result<List<ArticleMinimumDto>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<List<ArticleMinimumDto>>.Failure(ArticleErrors.LogMultipleNotFound, ex.Message);
        }
    }

    public async Task<IResult<ArticleDto>> GetByIdAsync(int id)
    {
        try
        {
            var result = await context.Articles
                    .AsNoTracking()
                    .Include(x => x.Images)
                    .Select(x => x.Map())
                    .FirstOrDefaultAsync(x => x.Id == id);

            return Result<ArticleDto>.Success(result!);
        }
        catch (Exception ex)
        {
            return Result<ArticleDto>.Failure(ArticleErrors.LogNotFound(nameof(id)), nameof(id), ex.Message);
        }
    }

    public async Task<IResult<PagedResultDto<ArticleMinimumDto>>> GetByCategory(int categoryId, int pageNumber, int pageSize)
    {
        try
        {
            var articles = await context.Articles
                    .AsNoTracking()
                    .Where(c => c.Categories!.Any(c => c.Id == categoryId))
                    .Select(x => x.MapMinimumDto())
                    .ToListAsync();

            var result = new PagedResultDto<ArticleMinimumDto>
            {
                Data = articles,
                TotalCount = articles.Count,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return Result<PagedResultDto<ArticleMinimumDto>>.Success(result!);
        }
        catch (Exception ex)
        {
            return Result<PagedResultDto<ArticleMinimumDto>>.Failure(ArticleErrors.LogMultipleNotFound, ex.Message);
        }
    }
}
