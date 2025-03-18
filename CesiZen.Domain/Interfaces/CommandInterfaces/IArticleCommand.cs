using CesiZen.Domain.Datamodel;
using CesiZen.Domain.Interface;

namespace CesiZen.Domain.Interfaces;

public interface IArticleCommand : ICommand<Article>
{
    Task<IResult> UpdateTitleAsync(Article article);

    Task<IResult> UpdateDescriptionAsync(Article article);

    Task<IResult> UpdateContentAsync(Article article);
}
