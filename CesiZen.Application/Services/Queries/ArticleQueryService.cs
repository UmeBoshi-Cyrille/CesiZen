using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using Serilog;

namespace CesiZen.Application.Services;

public class ArticleQueryService : AService, IArticleQueryService
{
    private readonly IArticleQuery query;

    public ArticleQueryService(ILogger logger, IArticleQuery query) : base(logger)
    {
        this.query = query;
    }

    public async Task<IResult<PagedResultDto<ArticleMinimumDto>>> GetAllAsync(int pageNumber, int pageSize)
    {
        var result = await query.GetAllAsync(pageNumber, pageSize);

        if (result.IsFailure)
        {
            logger.Error(result.Error.Message);
            return Result<PagedResultDto<ArticleMinimumDto>>.Failure(ArticleErrors.ClientMultipleNotFound);
        }

        return Result<PagedResultDto<ArticleMinimumDto>>.Success(result.Value);
    }

    public async Task<IResult<PagedResultDto<ArticleMinimumDto>>> SearchArticles(PageParametersDto parameters, string searchTerm = "")
    {
        var result = await query.SearchArticles(parameters, searchTerm);

        if (result.IsFailure)
        {
            logger.Error(result.Error.Message);
            return Result<PagedResultDto<ArticleMinimumDto>>.Failure(ArticleErrors.ClientMultipleNotFound);
        }

        return Result<PagedResultDto<ArticleMinimumDto>>.Success(result.Value);
    }

    public async Task<IResult<ArticleDto>> GetByIdAsync(int id)
    {
        var result = await query.GetByIdAsync(id);

        if (result.IsFailure)
        {
            logger.Error(result.Error.Message);
            return Result<ArticleDto>.Failure(ArticleErrors.ClientNotFound);
        }

        return Result<ArticleDto>.Success(result.Value);
    }

    public async Task<IResult<List<ArticleMinimumDto>>> GetLast(int amount)
    {
        var result = await query.GetLast(amount);

        if (result.IsFailure)
        {
            logger.Error(result.Error.Message);
            return Result<List<ArticleMinimumDto>>.Failure(ArticleErrors.ClientNotFound);
        }

        return Result<List<ArticleMinimumDto>>.Success(result.Value);
    }

    public async Task<IResult<PagedResultDto<ArticleMinimumDto>>> GetByCategory(int categoryId, int pageNumber, int pageSize)
    {
        var result = await query.GetByCategory(categoryId, pageNumber, pageSize);

        if (result.IsFailure)
        {
            logger.Error(result.Error.Message);
            return Result<PagedResultDto<ArticleMinimumDto>>.Failure(ArticleErrors.ClientNotFound);
        }

        return Result<PagedResultDto<ArticleMinimumDto>>.Success(result.Value);
    }
}
