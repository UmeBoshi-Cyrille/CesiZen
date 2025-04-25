using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Mapper;

public static class ImageMapper
{

    public static Image Map(this NewImageDto dto)
    {
        return new Image
        {
            Title = dto.Title,
            Alternative = dto.Alternative,
            Path = dto.Path,
        };
    }

    public static NewImageDto Map(this Image model)
    {
        return new NewImageDto
        {
            Title = model.Title,
            Alternative = model.Alternative,
            Path = model.Path,
        };
    }

    public static List<Image> Map(this List<NewImageDto> dto)
    {
        List<Image> model = new();

        for (var i = 0; i < dto.Count; i++)
        {
            var item = dto[i].Map();
            model.Add(item);
        }

        return model;
    }

    public static List<NewImageDto> Map(this List<Image> dto)
    {
        List<NewImageDto> model = new();

        for (var i = 0; i < dto.Count; i++)
        {
            var item = dto[i].Map();
            model.Add(item);
        }

        return model;
    }
}
