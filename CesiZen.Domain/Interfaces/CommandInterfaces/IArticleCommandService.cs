using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Interfaces;

public interface IArticleCommandService
{
    Task<IResult> UpdateTitleAsync(int id, string title);

    Task<IResult> UpdateDescriptionAsync(int id, string description);

    Task<IResult> UpdateContentAsync(int id, string content);

    Task<IResult> Insert(NewArticleDto entity);

    Task<IResult> Update(ArticleDto entity);

    Task<IResult> Delete(int id);
}
