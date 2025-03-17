using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
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

    public async Task<IResult<PagedResult<ArticleDto>>> GetAllAsync(int pageNumber, int pageSize)
    {
        var result = await query.GetAllAsync(pageNumber, pageSize);

        if (result.IsFailure)
        {
            logger.Error(result.Error.Message);
            return Result<PagedResult<ArticleDto>>.Failure(Error.NullValue(""));
        }

        var dto = result.Value.Map();

        return Result<PagedResult<ArticleDto>>.Success(dto);
    }

    public async Task<IResult<ArticleDto>> GetByIdAsync(int id)
    {
        var result = await query.GetByIdAsync(id);

        if (result.IsFailure)
        {
            logger.Error(result.Error.Message);
            return Result<ArticleDto>.Failure(Error.NullValue(""));
        }

        var dto = result.Value.Map();

        return Result<ArticleDto>.Success(dto);
    }

    public async Task<IResult<PagedResult<ArticleDto>>> SearchArticles(PageParameters parameters, string searchTerm = null)
    {
        var result = await query.SearchArticles(parameters, searchTerm);

        if (result.IsFailure)
        {
            logger.Error(result.Error.Message);
            return Result<PagedResult<ArticleDto>>.Failure(Error.NullValue(""));
        }

        var dto = result.Value.Map();

        return Result<PagedResult<ArticleDto>>.Success(dto);
    }
}
