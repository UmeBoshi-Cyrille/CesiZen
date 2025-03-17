using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Mapper;

public static class DataMapper
{
    #region Simple Mapper Methods
    public static User MapUser(this User user, UserDto dto, Authentifier authentifier, string emailVerificationToken)
    {
        Login login = new()
        {
            Email = dto.Email,
            Password = authentifier.Password,
            Salt = authentifier.HashSalt,
            EmailVerified = false,
            EmailVerificationToken = emailVerificationToken,
        };

        user.Firstname = dto.FirstName;
        user.Lastname = dto.LastName;
        user.UserName = dto.Username;
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
