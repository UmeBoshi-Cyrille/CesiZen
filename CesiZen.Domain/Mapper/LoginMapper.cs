using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Mapper;

public static class LoginMapper
{
    #region Map Methods
    public static Login Map(this AuthenticateRequestDto dto)
    {
        return new Login
        {
            Email = dto.Identifier,
            Password = dto.Password,
        };
    }

    public static LoginMinimumDto MapDto(this Login model)
    {
        return new LoginMinimumDto
        {
            Id = model.Id,
            Email = model.Email,
            EmailVerified = model.EmailVerified,
        };
    }

    //public static AuthenticateResponseDto Map(this CategoryDto dto)
    //{
    //    return new Category
    //    {
    //        Id = dto.Id,
    //        Name = dto.Name,
    //    };
    //}

    //public static CategoryDto Map(this Category model)
    //{
    //    return new CategoryDto
    //    {
    //        Name = model.Name,
    //    };
    //}

    public static AuthenticateRequestDto Map(this Login model)
    {
        return new AuthenticateRequestDto
        {
            Identifier = model.Email,
            Password = model.Password,
        };
    }
    #endregion

    //public static List<CategoryDto> Map(this List<Category> model)
    //{
    //    List<CategoryDto> dto = new();

    //    for (var i = 0; i < model.Count; i++)
    //    {
    //        var item = model[i].MapDto();
    //        dto.Add(item);
    //    }

    //    return dto;
    //}

    //public static List<Category> Map(this List<CategoryDto> dto)
    //{
    //    List<Category> model = new();

    //    for (var i = 0; i < dto.Count; i++)
    //    {
    //        var item = dto[i].Map();
    //        model.Add(item);
    //    }

    //    return model;
    //}

    //public static List<Category> Map(this List<CategoryDto> dto)
    //{
    //    List<Category> model = new();

    //    for (var i = 0; i < dto.Count; i++)
    //    {
    //        var item = dto[i].Map();
    //        model.Add(item);
    //    }

    //    return model;
    //}
}
