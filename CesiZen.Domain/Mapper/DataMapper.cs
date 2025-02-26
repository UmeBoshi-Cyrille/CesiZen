using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Mapper;

public static class DataMapper
{
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

        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.UserName = dto.Username;
        user.IsActive = true;
        user.Login = login;

        return user;
    }
}
