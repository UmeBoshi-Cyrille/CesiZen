using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;
using Microsoft.Extensions.Configuration;

namespace CesiZen.Domain.Mapper;

public static class UserMapper
{
    #region Simple Mapper Methods
    public static User MapAccountDto(this UserAccountDto dto)
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

    public static User Map(this AccountActivationDto dto)
    {
        User user = new();

        user.Id = dto.Id;
        user.IsActive = dto.IsActive;

        return user;
    }

    public static User Map(this UserMinimumDto dto)
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

    public static UserProfileDto MapProfileDto(this User model)
    {
        return new UserProfileDto
        {
            Firstname = model.Firstname,
            Lastname = model.Lastname,
            Username = model.Username,
            CreatedAt = model.CreatedAt,
            Email = model.Login!.Email,
        };
    }

    public static UserResponseDto MapResponseDto(this UserMinimumDto dto)
    {
        return new UserResponseDto
        {
            Id = dto.Id,
            Username = dto.Username,
            CreatedAt = dto.CreatedAt,
            Role = dto.Role,
            IsActive = dto.IsActive,
        };
    }

    public static UserResponseDto Map(this AuthenticationUserDto dto)
    {
        return new UserResponseDto
        {
            Id = dto.Id,
            Username = dto.Username,
            CreatedAt = dto.CreatedAt,
            Role = dto.Role,
            IsActive = dto.IsActive,
        };
    }

    public static UserResponseDto Map(this UserDto dto)
    {
        return new UserResponseDto
        {
            Id = dto.Id,
            Username = dto.Username,
            CreatedAt = dto.CreatedAt,
            Role = dto.Role,
            IsActive = dto.IsActive,
        };
    }

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

    public static UserMinimumDto MapMinimumDto(this User model)
    {
        return new UserMinimumDto
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

    public static UserDto MapDto(this User model)
    {
        return new UserDto
        {
            Id = model.Id,
            Firstname = model.Firstname,
            Lastname = model.Lastname,
            Username = model.Username,
            CreatedAt = model.CreatedAt,
            UpdatedAt = model.UpdatedAt,
            IsActive = model.IsActive,
            Role = model.Role,
            Login = model.Login!.MapDto()
        };
    }

    public static AuthenticationUserDto MapAuthenticationUserDto(this User model)
    {
        return new AuthenticationUserDto
        {
            Id = model.Id,
            Username = model.Username,
            CreatedAt = model.CreatedAt,
            IsActive = model.IsActive,
            Role = model.Role,
            Login = model.Login!.MapAuthenticationLoginDto(),
            RefreshToken = model.RefreshToken is null ? null : model.RefreshToken!.MapDto(),
            SessionId = model.Session is null ? string.Empty : model.Session!.SessionId
        };
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
    public static List<UserMinimumDto> Map(this List<User> model)
    {
        List<UserMinimumDto> dto = new();

        for (var i = 0; i < model.Count; i++)
        {
            var item = model[i].MapMinimumDto();
            dto.Add(item);
        }

        return dto;
    }

    public static List<User> Map(this List<UserMinimumDto> dto)
    {
        List<User> model = new();

        for (var i = 0; i < dto.Count; i++)
        {
            var item = dto[i].Map();
            model.Add(item);
        }

        return model;
    }

    public static List<User> Map(this List<UserAccountDto> dto)
    {
        List<User> model = new();

        for (var i = 0; i < dto.Count; i++)
        {
            var item = dto[i].MapAccountDto();
            model.Add(item);
        }

        return model;
    }
    #endregion

    #region Paginated Mapper Methods
    public static PagedResultDto<UserMinimumDto> Map(this PagedResultDto<User> model)
    {
        List<UserMinimumDto> dto = model.Data.Map();

        return new PagedResultDto<UserMinimumDto>
        {
            Data = dto,
            TotalCount = model.TotalCount,
            PageNumber = model.PageNumber,
            PageSize = model.PageSize,
        };
    }
    #endregion
}
