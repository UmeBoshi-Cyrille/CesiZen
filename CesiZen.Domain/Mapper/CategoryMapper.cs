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

    public static CategoryDto Map(this Category model)
    {
        return new CategoryDto
        {
            Name = model.Name,
        };
    }

    public static CategoryResponseDto MapResponseDto(this Category model)
    {
        return new CategoryResponseDto
        {
            Id = model.Id,
            Name = model.Name,
        };
    }

    public static CategoryDto MapDto(this Category model)
    {
        return new CategoryDto
        {
            Id = model.Id,
            Name = model.Name,
        };
    }
    #endregion

    public static List<CategoryDto> Map(this List<Category> model)
    {
        List<CategoryDto> dto = new();

        for (var i = 0; i < model.Count; i++)
        {
            var item = model[i].MapDto();
            dto.Add(item);
        }

        return dto;
    }

    public static List<CategoryResponseDto> MapResponseDto(this List<Category> model)
    {
        List<CategoryResponseDto> dto = new();

        for (var i = 0; i < model.Count; i++)
        {
            var item = model[i].MapResponseDto();
            dto.Add(item);
        }

        return dto;
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

    public static PagedResultDto<CategoryDto> Map(this PagedResultDto<Category> model)
    {
        List<CategoryDto> dto = model.Data.Map();

        return new PagedResultDto<CategoryDto>
        {
            Data = dto,
            TotalCount = model.TotalCount,
            PageNumber = model.PageNumber,
            PageSize = model.PageSize,
        };
    }
}
