using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Interfaces;

public interface IArticleQueryService
{
    Task<IResult<ArticleDto>> GetByIdAsync(int id);

    Task<IResult<PagedResultDto<ArticleMinimumDto>>> GetAllAsync(int pageNumber, int pageSize);

    Task<IResult<PagedResultDto<ArticleMinimumDto>>> SearchArticles(PageParametersDto parameters, string searchTerm = "");

    Task<IResult<List<ArticleMinimumDto>>> GetLast(int amount);

    Task<IResult<PagedResultDto<ArticleMinimumDto>>> GetByCategory(int categoryId, int pageNumber, int pageSize);
}
