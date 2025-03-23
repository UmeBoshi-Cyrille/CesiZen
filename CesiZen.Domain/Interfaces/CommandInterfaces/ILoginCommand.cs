using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Interfaces;

public interface ILoginCommand
{
    Task<IResult> UpdateEmailVerification(EmailVerificationDto dto);
    Task<IResult> UpdateEmail(int userId, string email);

    Task<IResult> UpdatePassword(int userId, string password);

    Task<IResult> ResetPassword(string token, string password);

    Task<IResult> UpdateResetPasswordToken(Login login);

    Task<IResult> UpdateSalt(int userId, string salt);

    Task<IResult> UpdateLoginAttempsCount(Login login);

    Task<IResult> UpdateLoginAttemps(Login login);
}
