using CesiZen.Domain.BusinessResult;
using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;
using CesiZen.Domain.Interfaces;
using CesiZen.Domain.Mapper;
using Serilog;

namespace CesiZen.Application.Services;

public class ArticleCommandService : AService, IArticleCommandService
{
    private readonly IArticleCommand command;

    public ArticleCommandService(ILogger logger, IArticleCommand command) : base(logger)
    {
        this.command = command;
    }

    public async Task<IResult> Insert(ArticleDto dto)
    {
        var article = dto.Map();
        article.CreatedAt = DateTime.Now;
        var result = await command.Insert(article);

        if (result.IsFailure)
        {
            logger.Error(result.Error.Message);
            return Result.Failure(Error.NullValue(""));
        }

        return Result.Success();
    }

    public async Task<IResult> Update(ArticleDto dto)
    {
        var article = dto.Map();
        article.UpdatedAt = DateTime.Now;
        var result = await command.Update(article);

        if (result.IsFailure)
        {
            logger.Error(result.Error.Message);
            return Result.Failure(Error.NullValue(""));
        }

        return Result.Success();
    }

    public async Task<IResult> UpdateTitleAsync(string id, string title)
    {
        var article = new Article() { Id = id, Title = title, UpdatedAt = DateTime.Now };

        var result = await command.UpdateTitleAsync(article);

        if (result.IsFailure)
        {
            logger.Error(result.Error.Message);
            return Result.Failure(Error.NullValue(""));
        }

        return Result.Success();
    }

    public async Task<IResult> UpdateDescriptionAsync(string id, string description)
    {
        var article = new Article() { Id = id, Description = description, UpdatedAt = DateTime.Now };
        var result = await command.UpdateDescriptionAsync(article);

        if (result.IsFailure)
        {
            logger.Error(result.Error.Message);
            return Result.Failure(Error.NullValue(""));
        }

        return Result.Success();
    }

    public async Task<IResult> UpdateContentAsync(string id, string content)
    {
        var article = new Article() { Id = id, Content = content, UpdatedAt = DateTime.Now };
        var result = await command.UpdateContentAsync(article);

        if (result.IsFailure)
        {
            logger.Error(result.Error.Message);
            return Result.Failure(Error.NullValue(""));
        }

        return Result.Success();
    }

    public async Task<IResult> Delete(string id)
    {
        var result = await command.Delete(id);

        if (result.IsFailure)
        {
            logger.Error(result.Error.Message);
            return Result.Failure(Error.NullValue(""));
        }

        return Result.Success();
    }
}
