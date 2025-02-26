using CesiZen.Domain.Datamodel;
using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Interface;

public interface ILoginCommand
{
    Task<IResult> UpdateLogin(EmailVerificationDto dto);
    Task<IResult> UpdateEmail(int userId, string email);

    Task<IResult> UpdatePassword(int userId, string password);

    Task<IResult> UpdateSalt(int userId, string salt);

    Task<IResult> UpdateLoginAttempsCount(Login login);

    Task<IResult> UpdateLoginAttemps(Login login);
}
