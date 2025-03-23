using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;

namespace CesiZen.Domain.Interfaces;

public interface IArticleCommandService : ICommand<ArticleDto>
{
    Task<IResult> UpdateTitleAsync(int Id, string title);

    Task<IResult> UpdateDescriptionAsync(int Id, string description);

    Task<IResult> UpdateContentAsync(int Id, string content);
}
