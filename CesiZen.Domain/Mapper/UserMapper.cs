using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;
using Microsoft.Extensions.Configuration;

namespace CesiZen.Domain.Mapper;

public static class UserMapper
{
    #region Simple Mapper Methods
    public static User Map(this NewUserDto dto, Authentifier authentifier, string emailVerificationToken, IConfiguration configuration)
    {
        User user = new();

        Login login = new()
        {
            Email = dto.Email,
            Password = authentifier.Password,
            Salt = authentifier.HashSalt,
            EmailVerified = false,
            EmailVerificationToken = emailVerificationToken,
        };

        user.Firstname = dto.Firstname;
        user.Lastname = dto.Lastname;
        user.Username = dto.Username;
        user.IsActive = true;
        user.Login = login;
        user.UpdatedAt = DateTime.UtcNow;
        user.Role = configuration["Roles:User"]!;

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

        user.Firstname = dto.Firstname;
        user.Lastname = dto.Lastname;
        user.Username = dto.Username;
        user.IsActive = true;
        user.Login = login;
        user.UpdatedAt = DateTime.UtcNow;

        return user;
    }

    public static User Map(this UserRequestDto dto)
    {
        return new User
        {
            Id = dto.Id,
            Firstname = dto.Firstname,
            Lastname = dto.Lastname,
            Username = dto.Username,
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
            Username = model.Username,
            CreatedAt = model.CreatedAt,
            UpdatedAt = model.UpdatedAt,
            IsActive = model.IsActive,
        };
    }

    public static User Map(this AccountActivationDto dto)
    {
        User user = new();

        user.Id = dto.Id;
        user.IsActive = dto.IsActive;

        return user;
    }

    public static EmailSenderDto MapEmailSender(string email, string template, string token, string subject)
    {
        return new EmailSenderDto()
        {
            Email = email,
            Template = template,
            Token = token,
            Subject = subject
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

    public static List<User> Map(this List<UserRequestDto> dto)
    {
        List<User> model = new();

        for (var i = 0; i < dto.Count; i++)
        {
            var item = dto[i].Map();
            model.Add(item);
        }

        return model;
    }

    public static List<User> Map(this List<UserDto> dto)
    {
        List<User> model = new();

        for (var i = 0; i < dto.Count; i++)
        {
            var item = dto[i].Map();
            model.Add(item);
        }

        return model;
    }
    #endregion

    #region Paginated Mapper Methods
    public static PagedResultDto<UserRequestDto> Map(this PagedResultDto<User> model)
    {
        List<UserRequestDto> dto = model.Data.Map();

        return new PagedResultDto<UserRequestDto>
        {
            Data = dto,
            TotalCount = model.TotalCount,
            PageNumber = model.PageNumber,
            PageSize = model.PageSize,
        };
    }
    #endregion
}
