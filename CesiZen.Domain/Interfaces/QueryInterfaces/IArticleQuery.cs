using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Interfaces;

public interface IArticleQuery : IQuery<ArticleDto, ArticleMinimumDto>
{
    Task<IResult<PagedResultDto<ArticleMinimumDto>>> SearchArticles(PageParametersDto parameters, string searchTerm = "");

    Task<IResult<List<ArticleMinimumDto>>> GetLast(int amount);

    Task<IResult<PagedResultDto<ArticleMinimumDto>>> GetByCategory(int categoryId, int pageNumber, int pageSize);
}
