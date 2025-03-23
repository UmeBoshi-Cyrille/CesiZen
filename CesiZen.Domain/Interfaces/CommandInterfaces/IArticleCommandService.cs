using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;

namespace CesiZen.Domain.Interfaces;

public interface IArticleCommandService : ICommand<ArticleDto>
{
    Task<IResult> UpdateTitleAsync(int id, string title);

    Task<IResult> UpdateDescriptionAsync(int id, string description);

    Task<IResult> UpdateContentAsync(int id, string content);
}
