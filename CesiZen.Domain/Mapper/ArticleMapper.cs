using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Mapper;

public static class ArticleMapper
{
    public static Article Map(this ArticleDto dto)
    {
        return new Article
        {
            Id = dto.Id,
            Title = dto.Title,
            Description = dto.Description,
            Author = dto.Author,
            Content = dto.Content,
            Images = dto.Images,
        };
    }

    public static ArticleDto Map(this Article model)
    {
        return new ArticleDto
        {
            Id = model.Id,
            Title = model.Title,
            Description = model.Description,
            Author = model.Author,
            Content = model.Content,
            Images = model.Images!,
        };
    }

    #region Collection Mapper Nethods
    public static List<ArticleDto> Map(this List<Article> model)
    {
        List<ArticleDto> dto = new();

        for (var i = 0; i < model.Count; i++)
        {
            var item = model[i].Map();
            dto.Add(item);
        }

        return dto;
    }

    public static List<Article> Map(this List<ArticleDto> dto)
    {
        List<Article> model = new();

        for (var i = 0; i < dto.Count; i++)
        {
            var item = dto[i].Map();
            model.Add(item);
        }

        return model;
    }
    #endregion

    #region Paginated Mapper Methods
    public static PagedResultDto<ArticleDto> Map(this PagedResultDto<Article> model)
    {
        List<ArticleDto> dto = model.Data.Map();

        return new PagedResultDto<ArticleDto>
        {
            Data = dto,
            TotalCount = model.TotalCount,
            PageNumber = model.PageNumber,
            PageSize = model.PageSize,
        };
    }
    #endregion
}
