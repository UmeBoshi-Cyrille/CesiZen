using CesiZen.Domain.Interfaces;

namespace CesiZen.Domain.Interfaces;

public interface ITokenProvider
{
    string GenerateAccessToken(string userId);

    Task<IResult<string>> RefreshAccessTokenAsync(string userId, string accessToken);

    Task<IResult> InvalidateTokens(string userId);

    bool CheckAccessTokenExpirationTime(string token);

    string GetTokenSessionId(string token);

    string GenerateVerificationToken();
}
