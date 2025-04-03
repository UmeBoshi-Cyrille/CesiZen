using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Mapper;

public static class CategoryMapper
{
    #region Map Methods
    public static Category Map(this CategoryDto dto)
    {
        return new Category
        {
            Id = dto.Id!,
            Name = dto.Name,
        };
    }

    public static Category Map(this CategoryRequestDto dto)
    {
        return new Category
        {
            Id = dto.Id,
            Name = dto.Name,
        };
    }

    public static CategoryDto Map(this Category model)
    {
        return new CategoryDto
        {
            Name = model.Name,
        };
    }

    public static CategoryRequestDto MapDto(this Category model)
    {
        return new CategoryRequestDto
        {
            Id = model.Id,
            Name = model.Name,
        };
    }
    #endregion

    public static List<CategoryRequestDto> Map(this List<Category> model)
    {
        List<CategoryRequestDto> dto = new();

        for (var i = 0; i < model.Count; i++)
        {
            var item = model[i].MapDto();
            dto.Add(item);
        }

        return dto;
    }

    public static List<Category> Map(this List<CategoryRequestDto> dto)
    {
        List<Category> model = new();

        for (var i = 0; i < dto.Count; i++)
        {
            var item = dto[i].Map();
            model.Add(item);
        }

        return model;
    }

    public static List<Category> Map(this List<CategoryDto> dto)
    {
        List<Category> model = new();

        for (var i = 0; i < dto.Count; i++)
        {
            var item = dto[i].Map();
            model.Add(item);
        }

        return model;
    }

    public static List<CategoryDto> MapDto(this List<Category> model)
    {
        List<CategoryDto> dto = new();

        for (var i = 0; i < model.Count; i++)
        {
            var item = model[i].Map();
            dto.Add(item);
        }

        return dto;
    }

    public static PagedResultDto<CategoryRequestDto> Map(this PagedResultDto<Category> model)
    {
        List<CategoryRequestDto> dto = model.Data.Map();

        return new PagedResultDto<CategoryRequestDto>
        {
            Data = dto,
            TotalCount = model.TotalCount,
            PageNumber = model.PageNumber,
            PageSize = model.PageSize,
        };
    }
}
