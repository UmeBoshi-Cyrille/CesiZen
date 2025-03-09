using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Interface;

public interface ILoginCommand
{
    Task<IResult> UpdateEmailVerification(EmailVerificationDto dto);
    Task<IResult> UpdateEmail(string userId, string email);

    Task<IResult> UpdatePassword(string userId, string password);

    Task<IResult> ResetPassword(string token, string password);

    Task<IResult> UpdateResetPasswordToken(Login login);

    Task<IResult> UpdateSalt(string userId, string salt);

    Task<IResult> UpdateLoginAttempsCount(Login login);

    Task<IResult> UpdateLoginAttemps(Login login);
}
