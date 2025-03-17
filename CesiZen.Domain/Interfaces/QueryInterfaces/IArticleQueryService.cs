using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interface;

namespace CesiZen.Domain.Interfaces;

public interface IArticleQueryService
{
    Task<IResult<PagedResult<ArticleDto>>> GetArticlesAsync(PageParameters parameters, string searchTerm = null);
    Task<IResult<ArticleDto>> GetByIdAsync(int id);
}
