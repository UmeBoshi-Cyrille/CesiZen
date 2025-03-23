using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Interfaces;

public interface IArticleQuery : IQuery<Article>
{
    Task<IResult<PagedResultDto<Article>>> SearchArticles(PageParametersDto parameters, string searchTerm = "");
}
