using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Interfaces;

public interface IPasswordService
{
    Authentifier HashPassword(string password, Login? login = null);

    bool IsCorrectPassword(Login login, string providedPassword);

    Task<IResult> ResetPassword(int userId, PasswordResetDto dto);

    Task<IResult<MessageEventArgs>> ForgotPasswordRequest(string email);

    Task<IResult> ForgotPasswordResponse(string email, string token);
}
