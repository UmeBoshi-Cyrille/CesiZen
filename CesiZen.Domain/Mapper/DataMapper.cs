using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Mapper;

public static class DataMapper
{
    #region Simple Mapper Methods
    public static Article Map(this ArticleDto dto)
    {
        return new Article
        {
            Id = dto.Id,
            Title = dto.Title,
            Description = dto.Description,
            Author = dto.Author,
            Content = dto.Content,
            Image = dto.Image,
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
            Image = model.Image,
            Images = model.Images,
        };
    }

    public static User Map(this UserDto dto, Authentifier authentifier, string emailVerificationToken)
    {
        User user = new();

        Login login = new()
        {
            Email = user.Email,
            Password = authentifier.Password,
            Salt = authentifier.HashSalt,
            EmailVerified = false,
            EmailVerificationToken = emailVerificationToken,
        };

        user.Firstname = user.Firstname;
        user.Lastname = user.Lastname;
        user.UserName = user.Username;
        user.IsActive = true;
        user.Login = login;

        return user;
    }

    public static User Map(this UserDto dto)
    {
        User user = new();

        Login login = new()
        {
            Email = dto.Email,
            Password = dto.Password,
            EmailVerified = false,
        };

        user.Firstname = user.Firstname;
        user.Lastname = user.Lastname;
        user.UserName = user.Username;
        user.IsActive = true;
        user.Login = login;

        return user;
    }

    public static User Map(this UserRequestDto dto)
    {
        return new User
        {
            Id = dto.Id,
            Firstname = dto.Firstname,
            Lastname = dto.Lastname,
            CreatedAt = dto.CreatedAt,
            UpdatedAt = dto.UpdatedAt,
            IsActive = dto.IsActive,
        };
    }

    public static UserRequestDto Map(this User model)
    {
        return new UserRequestDto
        {
            Id = model.Id,
            Firstname = model.Firstname,
            Lastname = model.Lastname,
            CreatedAt = model.CreatedAt,
            UpdatedAt = model.UpdatedAt,
            IsActive = model.IsActive,
        };
    }
    #endregion

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

    public static List<UserRequestDto> Map(this List<User> model)
    {
        List<UserRequestDto> dto = new();

        for (var i = 0; i < model.Count; i++)
        {
            var item = model[i].Map();
            dto.Add(item);
        }

        return dto;
    }
    #endregion


    #region Paginated Mapper Methods
    public static PagedResult<ArticleDto> Map(this PagedResult<Article> model)
    {
        List<ArticleDto> dto = model.Data.Map();

        return new PagedResult<ArticleDto>
        {
            Data = dto,
            TotalCount = model.TotalCount,
            PageNumber = model.PageNumber,
            PageSize = model.PageSize,
        };
    }

    public static PagedResult<UserRequestDto> Map(this PagedResult<User> model)
    {
        List<UserRequestDto> dto = model.Data.Map();

        return new PagedResult<UserRequestDto>
        {
            Data = dto,
            TotalCount = model.TotalCount,
            PageNumber = model.PageNumber,
            PageSize = model.PageSize,
        };
    }
    #endregion
}
