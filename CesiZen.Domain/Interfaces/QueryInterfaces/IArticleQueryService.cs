using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Interfaces;

public interface IArticleQueryService : IQueryServiceInterface<ArticleDto>
{
    Task<IResult<PagedResultDto<ArticleDto>>> SearchArticles(PageParametersDto parameters, string searchTerm = "");
}
