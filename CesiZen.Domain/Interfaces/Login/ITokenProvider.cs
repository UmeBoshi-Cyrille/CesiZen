using CesiZen.Domain.DataTransfertObject;
using System.Security.Claims;

namespace CesiZen.Domain.Interfaces;

public interface ITokenProvider
{
    string GenerateAccessToken(TokenBuilderDto dto);

    Task<IResult<TokenBuilderDto>> GenerateRefreshToken(int userId);

    Task<IResult<AuthenticateResponseDto>> RefreshAccessTokenAsync(string accessToken, ClaimsPrincipal principal);

    Task<IResult> InvalidateTokens(int userId);

    string GenerateVerificationToken();

    string? GetSessionId(ClaimsPrincipal principal);
}
