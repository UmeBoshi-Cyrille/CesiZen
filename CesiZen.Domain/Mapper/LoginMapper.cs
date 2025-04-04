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

    public static Login MapLoginAccess(this AuthenticationLoginDto dto)
    {
        return new Login
        {
            Id = dto.Id,
            AccessFailedCount = dto.AccessFailedCount,
            AccountIsLocked = dto.AccountIsLocked,
            LockoutEndTime = dto.LockoutEndTime,
        };
    }

    //public static Login MapLoginAccess(this AuthenticationLoginDto dto)
    //{
    //    return new Login
    //    {
    //        Id = dto.Id,
    //        Email = dto.Email,
    //        Password = dto.Password,
    //        Salt = dto.Salt,
    //        AccessFailedCount = dto.AccessFailedCount,
    //        AccountIsLocked = dto.AccountIsLocked,
    //        LockoutEndTime = dto.LockoutEndTime,
    //    };
    //}

    public static LoginMinimumDto MapDto(this Login model)
    {
        return new LoginMinimumDto
        {
            Id = model.Id,
            Email = model.Email,
            EmailVerified = model.EmailVerified,
        };
    }

    public static AuthenticationLoginDto MapAuthenticationLoginDto(this Login model)
    {
        return new AuthenticationLoginDto
        {
            Id = model.Id,
            Email = model.Email,
            Password = model.Password,
            Salt = model.Salt,
            AccessFailedCount = model.AccessFailedCount,
            AccountIsLocked = model.AccountIsLocked,
            LockoutEndTime = model.LockoutEndTime,
        };
    }

    public static AuthenticateRequestDto Map(this Login model)
    {
        return new AuthenticateRequestDto
        {
            Identifier = model.Email,
            Password = model.Password,
        };
    }
    #endregion
}
