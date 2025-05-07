using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Interfaces;

public interface IPasswordService
{
    Authentifier HashPassword(string password, string hashSalt = "");

    bool IsCorrectPassword(string hashSalt, string currentPassword, string providedPassword);

    Task<IResult> ResetPassword(int userId, PasswordResetDto dto);

    Task<IResult> ResetForgottenPassword(int userId, ResetForgottenPasswordDto dto);

    Task<IResult<MessageEventArgs>> ForgotPasswordRequest(string email);

    Task<IResult<int>> ForgotPasswordResponse(string email, string token);
}
