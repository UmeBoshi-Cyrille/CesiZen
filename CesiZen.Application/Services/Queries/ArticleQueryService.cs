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

    public async Task<IResult<PagedResultDto<ArticleDto>>> GetAllAsync(int pageNumber, int pageSize)
    {
        var result = await query.GetAllAsync(pageNumber, pageSize);

        if (result.IsFailure)
        {
            logger.Error(result.Error.Message);
            return Result<PagedResultDto<ArticleDto>>.Failure(ArticleErrors.ClientMultipleNotFound);
        }

        var dto = result.Value.Map();

        return Result<PagedResultDto<ArticleDto>>.Success(dto);
    }

    public async Task<IResult<ArticleDto>> GetByIdAsync(int Id)
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

    public async Task<IResult<PagedResultDto<ArticleDto>>> SearchArticles(PageParametersDto parameters, string searchTerm = "")
    {
        var result = await query.SearchArticles(parameters, searchTerm);

        if (result.IsFailure)
        {
            logger.Error(result.Error.Message);
            return Result<PagedResultDto<ArticleDto>>.Failure(ArticleErrors.ClientMultipleNotFound);
        }

        var dto = result.Value.Map();

        return Result<PagedResultDto<ArticleDto>>.Success(dto);
    }
}
