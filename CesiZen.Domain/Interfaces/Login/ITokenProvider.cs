using CesiZen.Domain.DataTransfertObject;

namespace CesiZen.Domain.Interfaces;

public interface ITokenProvider
{
    string GenerateAccessToken(TokenIdDto dto);

    IResult<TokenIdDto> GenerateRefreshToken(int userId);

    Task<IResult<string>> RefreshAccessTokenAsync(string accessToken);

    Task<IResult> InvalidateTokens(int userId);

    string GenerateVerificationToken();

    string? GetSessionId(string token);
}
