using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;

namespace CesiZen.Domain.Interfaces;

public interface IPasswordService
{
    Authentifier HashPassword(string password, Login? login = null);

    bool VerifyPassword(Login login, string providedPassword);

    Task<IResult> ResetPassword(PasswordResetDto dto);

    Task<IResult> ForgotPassword(PasswordResetRequestDto request);
}
