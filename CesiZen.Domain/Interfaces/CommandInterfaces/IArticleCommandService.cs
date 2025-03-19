using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;

namespace CesiZen.Domain.Interfaces;

public interface IArticleCommandService : ICommand<ArticleDto>
{
    Task<IResult> UpdateTitleAsync(string id, string title);

    Task<IResult> UpdateDescriptionAsync(string id, string description);

    Task<IResult> UpdateContentAsync(string id, string content);
}
