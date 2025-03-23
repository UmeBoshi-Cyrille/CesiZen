using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Interfaces;

public interface IPasswordService
{
    Authentifier HashPassword(string password, Login? login = null);

    bool VerifyPassword(Login login, string providedPassword);

    Task<IResult> ResetPassword(PasswordResetDto dto);

    Task<IResult<MessageEventArgs>> ForgotPassword(PasswordResetRequestDto request);
}
