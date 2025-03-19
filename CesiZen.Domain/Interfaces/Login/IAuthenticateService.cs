using CesiZen.Domain.DataTransfertObject;
using CesiZen.Domain.Interfaces;

namespace CesiZen.Domain.Interfaces;

public interface IAuthenticateService
{
    Task<IResult<AuthenticateResponseDto>> Authenticate(AuthenticateRequestDto model);

    Task<IResult> VerifyEmail(string token, string email);

    Task<IResult> Disconnect(string accessToken);
}
