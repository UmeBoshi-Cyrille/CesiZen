using CesiZen.Domain.Datamodel;

namespace CesiZen.Domain.Interfaces;

public interface ILoginQuery
{
    Task<IResult<Login>> GetByUserId(int userId);

    Task<IResult<Login>> GetByEmail(string email);

    Task<IResult<Login>> GetByResetPasswordToken(string token);

    Task<IResult> CheckEmail(string email);

    Task<IResult<ResetPassword>> GetResetPassword(string email, string token);
}