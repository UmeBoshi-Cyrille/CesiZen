using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Interfaces;

public interface IArticleCommand : ICommand<ArticleMinimumDto, Article>
{
    Task<IResult> UpdateTitleAsync(Article article);

    Task<IResult> UpdateDescriptionAsync(Article article);

    Task<IResult> UpdateContentAsync(Article article);
}
