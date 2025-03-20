using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Interfaces;

public interface IArticleQueryService : IQueryServiceInterface<ArticleDto>
{
    Task<IResult<PagedResult<ArticleDto>>> SearchArticles(PageParameters parameters, string searchTerm = "");
}
