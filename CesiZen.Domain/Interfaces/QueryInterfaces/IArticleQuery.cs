using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interface;

namespace CesiZen.Domain.Interfaces;

public interface IArticleQuery : IQuery<Article>
{
    Task<IResult<PagedResult<Article>>> SearchArticles(PageParameters parameters, string searchTerm = null);
}
