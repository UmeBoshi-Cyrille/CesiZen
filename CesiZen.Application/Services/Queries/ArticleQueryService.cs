using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using CesiZen.Domain.Mapper;
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

        var dto = result.Value.Map();

        return Result<PagedResultDto<ArticleMinimumDto>>.Success(dto);
    }

    public async Task<IResult<PagedResultDto<ArticleMinimumDto>>> SearchArticles(PageParametersDto parameters, string searchTerm = "")
    {
        var result = await query.SearchArticles(parameters, searchTerm);

        if (result.IsFailure)
        {
            logger.Error(result.Error.Message);
            return Result<PagedResultDto<ArticleMinimumDto>>.Failure(ArticleErrors.ClientMultipleNotFound);
        }

        var dto = result.Value.Map();

        return Result<PagedResultDto<ArticleMinimumDto>>.Success(dto);
    }

    public async Task<IResult<ArticleDto>> GetByIdAsync(int id)
    {
        var result = await query.GetByIdAsync(id);

        if (result.IsFailure)
        {
            logger.Error(result.Error.Message);
            return Result<ArticleDto>.Failure(ArticleErrors.ClientNotFound);
        }

        var dto = result.Value.Map();

        return Result<ArticleDto>.Success(dto);
    }

    public async Task<IResult<List<ArticleMinimumDto>>> GetLast(int amount)
    {
        var result = await query.GetLast(amount);

        if (result.IsFailure)
        {
            logger.Error(result.Error.Message);
            return Result<List<ArticleMinimumDto>>.Failure(ArticleErrors.ClientNotFound);
        }

        var dto = result.Value.Map();

        return Result<List<ArticleMinimumDto>>.Success(dto);
    }
}
