using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Interfaces;

public interface ILoginQuery
{
    Task<IResult<AuthenticationLoginDto>> GetByUserId(int userId);

    Task<IResult<Login>> GetByEmail(string email);

    Task<IResult<int>> GetUserIdByEmail(string email);

    Task<IResult<Login>> GetByResetPasswordToken(string token);

    Task<IResult> CheckEmail(string email);

    Task<IResult<ResetPasswordDto>> GetResetPassword(string email, string token);

    Task<IResult<Login>> GetByEmailVerificationToken(string token);
}