using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Interfaces;

public interface IAuthenticateService
{
    Task<IResult<AuthenticateResponseDto>> Authenticate(AuthenticateRequestDto dto);

    Task<IResult> VerifyEmail(string token, string email);

    Task<IResult> Disconnect(int userId);
}
