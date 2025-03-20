using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Interfaces;

public interface IArticleQuery : IQuery<Article>
{
    Task<IResult<PagedResult<Article>>> SearchArticles(PageParameters parameters, string searchTerm = "");
}
