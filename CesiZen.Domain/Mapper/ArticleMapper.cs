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
            ImagePath = dto.ImagePath,
            Images = dto.Images is not null ? dto.Images.ToList().Map() : new List<Image>(),
            UpdatedAt = DateTime.UtcNow
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
            ImagePath = model.ImagePath,
            Images = model.Images is not null ? model.Images!.ToList().Map()! : new List<ImageDto>(),
            UpdatedAt = model.UpdatedAt,
            CreatedAt = model.CreatedAt
        };
    }

    public static Article MapNew(this NewArticleDto dto)
    {
        return new Article
        {
            Title = dto.Title,
            Description = dto.Description,
            Author = dto.Author,
            Content = dto.Content,
            ImagePath = dto.ImagePath,
            Images = dto.Images is not null ? dto.Images.ToList().Map() : new List<Image>(),
            Categories = dto.Images is not null ? dto.Categories!.ToList().Map() : new List<Category>(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public static ArticleMinimumDto MapMinimumDto(this Article model)
    {
        return new ArticleMinimumDto
        {
            Id = model.Id,
            Title = model.Title,
            Description = model.Description,
            Author = model.Author,
            CreatedAt = model.CreatedAt,
            ImagePath = model.ImagePath,
            Categories = model.Categories!.ToList().MapDto()
        };
    }

    #region Collection Mapper Nethods
    public static List<ArticleMinimumDto> Map(this List<Article> model)
    {
        List<ArticleMinimumDto> dto = new();

        for (var i = 0; i < model.Count; i++)
        {
            var item = model[i].MapMinimumDto();
            dto.Add(item);
        }

        return dto;
    }

    public static List<Article> Map(this List<NewArticleDto> dto)
    {
        List<Article> model = new();

        for (var i = 0; i < dto.Count; i++)
        {
            var item = dto[i].MapNew();
            model.Add(item);
        }

        return model;
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
    public static PagedResultDto<ArticleMinimumDto> Map(this PagedResultDto<Article> model)
    {
        List<ArticleMinimumDto> dto = model.Data.Map();

        return new PagedResultDto<ArticleMinimumDto>
        {
            Data = dto,
            TotalCount = model.TotalCount,
            PageNumber = model.PageNumber,
            PageSize = model.PageSize,
        };
    }
    #endregion
}
